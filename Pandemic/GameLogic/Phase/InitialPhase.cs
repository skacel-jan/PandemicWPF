using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class InitialPhase : IGamePhase
    {
        public InitialPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public Game Game { get; }

        public void Action(string actionType)
        {
            
        }

        public void End()
        {
        }

        public void Start()
        {
            BuildResearchStation(Game.WorldMap.GetCity(City.Atlanta));

            InitialInfection();
            InitialDraw();
            AddEpidemicCards(5);

            Game.GamePhase = new ActionPhase(Game);
        }

        private void AddEpidemicCards(int epidemicCardsCount)
        {
            Game.PlayerDeck.AddEpidemicCards(epidemicCardsCount);
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
                foreach (var i in Enumerable.Range(0, cardCount))
                {
                    Card card = Game.PlayerDeck.DrawTop();
                    character.AddCard(card);
                    if (card is EventCard eventCard)
                    {
                        Game.EventCards.Add(eventCard);
                    }
                }
            }
        }

        private void InitialInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                foreach (var x in Enumerable.Range(0, 3))
                {
                    InfectionCard infectionCard = Game.InfectionDeck.DrawTop();
                    Game.InfectionDiscardPile.Cards.Add(infectionCard);
                    int changeInfections = Game.WorldMap.GetCity(infectionCard.City.Name).ChangeInfection(infectionCard.City.Color, i);
                    Game.DecreaseCubePile(infectionCard.City.Color, changeInfections);
                }
            }
        }
    }
}
