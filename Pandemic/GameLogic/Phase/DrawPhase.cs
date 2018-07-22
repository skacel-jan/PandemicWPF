using Pandemic.Cards;
using Pandemic.Decks;
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

        public void Action(string actionType)
        {
            if (!TryDrawPlayerCard(Game.CurrentCharacter, out Card firstCard))
            {
                GameOver();
            }

            _drawnCards.Add(firstCard);

            if (!TryDrawPlayerCard(Game.CurrentCharacter, out Card secondCard))
            {
                GameOver();
            }
            _drawnCards.Add(secondCard);

            if (Game.CurrentCharacter.HasMoreCardsThenLimit)
            {
                var selectedCards = new List<Card>();
                var callback = new Func<Card, bool>((Card card) =>
                {
                    if (selectedCards.Contains(card))
                    {
                        selectedCards.Remove(card);
                    }
                    else
                    {
                        selectedCards.Add(card);
                    }

                    if (Game.CurrentCharacter.CardsLimit >= (Game.CurrentCharacter.Cards.Count - selectedCards.Count))
                    {
                        foreach (var playerCard in selectedCards)
                        {
                            Game.CurrentCharacter.RemoveCard(playerCard);
                            Game.AddCardToPlayerDiscardPile(playerCard);
                        }

                        Game.Info = new GameInfo($"Cards discarded: {string.Join<string>(",", selectedCards.Select(c => c.Name))}.", "Infection phase",
                        () => Game.DoAction("Next"));

                        NextPhase();
                    }
                    return false;
                });                

                Game.SelectCard(Game.CurrentCharacter.Cards, callback, $"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}.{Environment.NewLine}" +
                        $"Player has more cards then his hand limit. Card has to be discarded.");
            }
            else
            {
                Game.Info = new GameInfo($"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}.", "Infection phase",
                    () => Game.DoAction("Next"));


                NextPhase();
            }
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
            Game.Info = new GameInfo($"Outbreak : {e.City}", string.Empty, null);
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

            InfectionCard card = Game.InfectionDeck.DrawBottom();
            Game.InfectionDiscardPile.Cards.Add(card);

            if (Game.IsCubePileEmpty(card.City.Color))
            {
                GameOver();
            }
            else
            {
                if (CanRaiseInfection(Game.WorldMap.GetCity(card.City.Name), card.City.Color))
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

                foreach (var connectedCity in Game.WorldMap.GetCity(outbreakCity.Name).ConnectedCities)
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
            var newDeck = new Deck<InfectionCard>(Game.InfectionDiscardPile.Cards);
            newDeck.Shuffle();
            newDeck.AddCards(Game.InfectionDeck.Cards);
            Game.InfectionDeck = newDeck;
            Game.InfectionDiscardPile.Cards.Clear();
        }

        private bool TryDrawPlayerCard(Character character, out Card card)
        {
            bool isGameOver = false;
            card = Game.PlayerDeck.DrawTop();

            if (card == null)
            {
                isGameOver = true;
            }

            if (card is PlayerCard)
            {
                character.AddCard(card);
            }
            else if (card is EventCard eventCard)
            {
                character.AddCard(card);
                Game.EventCards.Add(eventCard);
            }
            else if (card is EpidemicCard)
            {
                DoEpidemicActions();
            }

            return !isGameOver;
        }
    }
}