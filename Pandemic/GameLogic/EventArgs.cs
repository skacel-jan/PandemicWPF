using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class CardsSelectingEventArgs : EventArgs
    {
        public CardsSelectingEventArgs(int cardsCount, string text)
        {
            CardsCount = cardsCount;
            Text = text;
        }

        public string Text { get; }
        public int CardsCount { get; }
    }

    public class CureDiscoveredEventArgs : EventArgs
    {
        public CureDiscoveredEventArgs(DiseaseColor color, IEnumerable<CityCard> cards)
        {
            Color = color;
            Cards = cards ?? throw new ArgumentNullException(nameof(cards));
        }

        public IEnumerable<CityCard> Cards { get; }
        public DiseaseColor Color { get; }
    }

    public class InfoTextEventArgs : EventArgs
    {
        public InfoTextEventArgs(string infoText)
        {
            InfoText = infoText;
        }

        public string InfoText { get; }
    }

    public class StructureBuiltEventArgs : EventArgs
    {
        public StructureBuiltEventArgs(PlayerCard card)
        {
            Card = card;
        }

        public PlayerCard Card { get; }
    }

    public class TreatDiseaseEventArgs : EventArgs
    {
        public TreatDiseaseEventArgs(int cubesCount, DiseaseColor diseaseColor)
        {
            CubesCount = cubesCount;
            DiseaseColor = diseaseColor;
        }

        public int CubesCount { get; }
        public DiseaseColor DiseaseColor { get; }
    }
}