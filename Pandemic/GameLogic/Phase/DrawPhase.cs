using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic
{
    public class DrawPhase : IGamePhase
    {
        private IList<Card> _drawnCards;

        public DrawPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            _drawnCards = new List<Card>(2);
        }

        public Game Game { get; }

        public void Action(IGameAction action)
        {
            if (!TryDrawPlayerCard(Game.CurrentCharacter, out PlayerCard firstCard))
            {
                GameOver();
            }

            _drawnCards.Add(firstCard);

            if (!TryDrawPlayerCard(Game.CurrentCharacter, out PlayerCard secondCard))
            {
                GameOver();
            }
            _drawnCards.Add(secondCard);

            if (Game.CurrentCharacter.HasMoreCardsThenLimit)
            {
                Game.SetInfo($"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}.{Environment.NewLine}" +
                        $"Player has more cards then his hand limit. Card has to be discarded.");

                var selectAction = new MultiSelectAction<IEnumerable<PlayerCard>>(SetCard, Game.CurrentCharacter.Cards, string.Empty, ValidateCards);
                Game.SelectionService.Select(selectAction);
            }
            else
            {
                Game.SetInfo($"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}.", "Infection phase",
                    () => Game.DoAction(null));

                NextPhase();
            }
        }

        private bool ValidateCards(IEnumerable<PlayerCard> cards)
        {
            return Game.CurrentCharacter.CardsLimit >= (Game.CurrentCharacter.Cards.Count - cards.Count());
        }

        private void SetCard(IEnumerable<PlayerCard> cards)
        {
            foreach (var playerCard in cards)
            {
                Game.CurrentCharacter.RemoveCard(playerCard);
                Game.AddCardToPlayerDiscardPile(playerCard);
            }

            Game.SetInfo($"Cards discarded: {string.Join<string>(",", cards.Select(c => c.Name))}.", "Infection phase",
                () => Game.DoAction(null));

            NextPhase();
        }

        public void End()
        {
        }

        public void Start()
        {
        }

        protected void OnOutbreak(OutbreakEventArgs e)
        {
            Game.Outbreaks++;
            Game.SetInfo($"Outbreak : {e.City}", string.Empty, null);
        }

        private bool CanRaiseInfection(MapCity city, DiseaseColor color)
        {
            bool result = true;

            foreach (var character in Game.Characters)
            {
                result = result && !character.CanPreventInfection(city, color);
            }

            return result;
        }

        private void DoEpidemicActions()
        {
            Game.Infection.IncreasePosition();

            InfectionCard card = Game.Infection.Deck.Draw(DeckSide.Top);
            Game.Infection.DiscardPile.AddCard(card);

            if (Game.IsCubePileEmpty(card.City.Color))
            {
                GameOver();
            }
            else
            {
                if (CanRaiseInfection(Game.WorldMap[card.City.Name], card.City.Color))
                {
                    var isOutbreak = Game.IncreaseInfection(card.City, card.City.Color);
                    if (isOutbreak)
                    {
                        DoOutbreak(card.City, card.City.Color);
                    }
                }
            }

            ShuffleInfectionDiscardPileBack();
        }

        private void DoOutbreak(City city, DiseaseColor diseaseColor)
        {
            var citiesToOutbreak = new Queue<City>(1);
            var alreadyOutbreakedCities = new List<City>();
            citiesToOutbreak.Enqueue(city);

            OnOutbreak(new OutbreakEventArgs(city));

            while (citiesToOutbreak.Count > 0)
            {
                var outbreakCity = citiesToOutbreak.Dequeue();
                alreadyOutbreakedCities.Add(outbreakCity);

                foreach (var connectedCity in Game.WorldMap[outbreakCity.Name].ConnectedCities)
                {
                    if (CanRaiseInfection(connectedCity, diseaseColor))
                    {
                        bool isOutbreak = Game.IncreaseInfection(connectedCity.City, diseaseColor);
                        if (Game.IsCubePileEmpty(city.Color))
                        {
                            GameOver();
                        }

                        if (isOutbreak && !alreadyOutbreakedCities.Contains(connectedCity.City) && !citiesToOutbreak.Contains(connectedCity.City))
                        {
                            citiesToOutbreak.Enqueue(connectedCity.City);
                            OnOutbreak(new OutbreakEventArgs(connectedCity.City));
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            Game.ChangeGamePhase(new GameOverPhase(Game));
        }

        private void NextPhase()
        {
            Game.ChangeGamePhase(new InfectionPhase(Game));
        }

        private void ShuffleInfectionDiscardPileBack()
        {
            Game.Infection.ShuffleDiscardPileToDeck();
        }

        private bool TryDrawPlayerCard(Character character, out PlayerCard card)
        {
            bool isGameOver = false;
            card = Game.PlayerDeck.Draw(DeckSide.Top);

            if (card == null)
            {
                isGameOver = true;
            }

            if (card is EventCard eventCard)
            {
                character.AddCard(card);
            }
            else if (card is EpidemicCard)
            {
                DoEpidemicActions();
            }
            else
            {
                character.AddCard(card);
            }

            return !isGameOver;
        }
    }
}