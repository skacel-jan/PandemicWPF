using PandemicLegacy.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public class Board
    {
        public int InfectionRate { get; private set; }
        public int InfectionPosition { get; private set; }
        public int Outbreaks { get; private set; }
        public InfectionDeck InfectionDeck { get; private set; }
        public InfectionDeck InfectionDiscardPile { get; private set; }
        public PlayerDeck PlayerDeck { get; private set; }
        public PlayerDeck PlayerDiscardPile { get; private set; }
        public WorldMap WorldMap { get; private set; }

        public Board(WorldMap map, InfectionDeck infectionDeck, PlayerDeck playerDeck)
        {
            WorldMap = map;
            InfectionDeck = infectionDeck;
            PlayerDeck = playerDeck;

            Outbreaks = 0;
            InfectionRate = 2;
            InfectionPosition = 0;

            InfectionDiscardPile = new InfectionDeck(new List<InfectionCard>());
            PlayerDiscardPile = new PlayerDeck(new List<PlayerCard>());
        }

        public void RaiseInfection()
        {
            InfectionPosition++;
            if (InfectionPosition == 3 || InfectionPosition == 5)
            {
                InfectionRate++;
            }
        }
    }
}
