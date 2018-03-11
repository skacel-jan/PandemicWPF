using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Pandemic
{
    public static class MessageTokens
    {
        public const string StartNewGame = "StartNewGame";
        public const string CitySelected = "CitySelected";
        public const string CardSelected = "CardSelected";
        public const string DiseaseSelected = "DiseaseSelected";
        public const string MultipleCardsSelected = "CardsSelected";
        public const string MoveSelected = "MoveSelected";
        public const string InstantMove = "InstantMove";
        public const string MoveAction = "MoveAction";
        public const string DiscardAction = "DiscardAction";
        public const string ResearchStation = "ResearchStation";
    }
}
