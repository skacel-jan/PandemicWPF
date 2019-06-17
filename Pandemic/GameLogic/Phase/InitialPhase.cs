using Pandemic.Cards;
using Pandemic.Decks;
using Pandemic.GameLogic.Actions;
using System;
using System.Linq;

namespace Pandemic.GameLogic
{
    public class InitialPhase : IGamePhase
    {
        public InitialPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public Game Game { get; }

        public void Continue()
        {
        }

        public void End()
        {
        }

        public void Start()
        {
            BuildResearchStation(Game.WorldMap[City.Atlanta]);

            InitialInfection();
            InitialDraw();
            Game.PlayerDeck.AddEpidemicCards(Game.Difficulty);

            Game.ChangeGamePhase(new ActionPhase(Game));
        }

        private void BuildResearchStation(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            Game.ResearchStationsPile--;
        }

        private void InitialDraw()
        {
            int cardCount = 6 - Game.Characters.Count();
            foreach (var character in Game.Characters)
            {
                foreach (var _ in Enumerable.Range(0, cardCount))
                {
                    PlayerCard card = Game.PlayerDeck.Draw(DeckSide.Top);
                    character.AddCard(card);
                }
            }
        }

        private void InitialInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                foreach (var x in Enumerable.Range(0, 3))
                {
                    InfectionCard infectionCard = Game.Infection.Deck.Draw(DeckSide.Top);
                    Game.Infection.DiscardPile.AddCard(infectionCard);
                    int changeInfections = Game.WorldMap[infectionCard.City.Name].ChangeInfection(infectionCard.City.Color, i);
                    Game.DecreaseCubePile(infectionCard.City.Color, changeInfections);
                }
            }
        }
    }
}