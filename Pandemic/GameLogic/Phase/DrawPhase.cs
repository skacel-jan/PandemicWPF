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
        private readonly IList<Card> _drawnCards;

        private readonly IList<ActionState> m_actionStates;
        private readonly IList<ActionState> m_currentActionStates = new List<ActionState>();

        public Game Game { get; }

        public DrawPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            _drawnCards = new List<Card>(2);
            m_actionStates = new List<ActionState>
            {
                new ActionState(
                   (g) => g.PlayerDeck.Cards.Count() <= 1,
                   (g) => GameOver(),
                   Game),

                new ActionState(
                    (g) => true,
                    (g) =>
                    {
                        _drawnCards.Add(DrawPlayerCard(Game.CurrentCharacter));
                        Continue();
                    },
                    Game),

                new ActionState(
                    (g) => true,
                    (g) =>
                    {
                        _drawnCards.Add(DrawPlayerCard(Game.CurrentCharacter));
                        Continue();
                    },
                    Game),

                new ActionState(
                    (g) => Game.CurrentCharacter.HasMoreCardsThenLimit,
                    (g) => Game.SetInfo($"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}", "Continue",
                                        () => Game.ResolveAction(new DiscardPlayerCardAction(Game.CurrentCharacter, Game))),
                    Game),

                new ActionState(
                    (g) => !Game.CurrentCharacter.HasMoreCardsThenLimit,
                    (g) => Game.SetInfo($"Drawn cards: {_drawnCards[0].Name} and {_drawnCards[1].Name}.", "Infection phase",
                           () => Continue()),
                    Game),

                new ActionState(
                    (g) => true,
                    (g) => NextPhase(),
                    Game)
            };
        }

        public void Continue()
        {
            while (true)
            {
                var actionState = m_currentActionStates.First();
                m_currentActionStates.RemoveAt(0);
                if (actionState.Predicate(Game))
                {
                    actionState.Execute();
                    break;
                }
            }            
        }

        public void End()
        {
        }

        public void Start()
        {
            foreach (var state in m_actionStates)
            {
                m_currentActionStates.Add(state);
            }
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

        private PlayerCard DrawPlayerCard(Character character)
        {
            var card = Game.PlayerDeck.Draw(DeckSide.Top);

            if (card is EpidemicCard)
            {
                DoEpidemicActions();
            }
            else
            {
                character.AddCard(card);
            }

            return card;
        }

        private void GameOver()
        {
            Game.ChangeGamePhase(new GameOverPhase(Game));
        }

        private void NextPhase()
        {
            Game.ChangeGamePhase(new InfectionPhase(Game));
            Game.Continue();
        }

        private void ShuffleInfectionDiscardPileBack()
        {
            Game.Infection.ShuffleDiscardPileToDeck();
        }
    }
}