using Pandemic.Cards;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic
{
    public class InfectionPhase : IGamePhase
    {
        public Game Game { get; }

        public InfectionPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Action(IGameAction action)
        {
            if (Game.Infection.Actual == 0)
            {
                Game.Info = null;
                Game.ChangeGamePhase(new ActionPhase(Game));
                return;
            }

            Game.Infection.Actual--;

            InfectionCard card = Game.Infection.Deck.Draw(Decks.DeckSide.Top);
            Game.Infection.DiscardPile.AddCard(card);

            if (Game.IsCubePileEmpty(card.City.Color))
            {
                GameOver();
            }
            else
            {
                if (CanRaiseInfection(Game.WorldMap.GetCity(card.City.Name), card.City.Color))
                {
                    Game.Info = new GameInfo($"Infected city {card.City.Name}", $"Next {(Game.Infection.Actual == 0 ? "Player" : "Infection")}",
                        () => Game.DoAction(null));

                    var isOutbreak = Game.IncreaseInfection(card.City, card.City.Color);
                    if (isOutbreak)
                    {
                        DoOutbreak(card.City, card.City.Color);
                    }
                }
                else
                {
                    Game.Info = new GameInfo($"City {card.City.Name} was not infected", $"Next {(Game.Infection.Actual == 0 ? "Player" : "Infection")}",
                        () => Game.DoAction(null));
                }
            }
        }

        public void End()
        {
            Game.Characters.Next();
        }

        public void Start()
        {
            Game.Infection.Reset();
        }

        protected void OnOutbreak(OutbreakEventArgs e)
        {
            Game.Outbreaks++;
            Game.Info = new GameInfo(Game.Info.Text + $"\r\nOutbreak in city: {e.City.Name}", Game.Info.ButtonText, Game.Info.Action);
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
            if (!(Game.GamePhase is GameOverPhase))
            {
                Game.ChangeGamePhase(new GameOverPhase(Game));
            }
        }
    }
}