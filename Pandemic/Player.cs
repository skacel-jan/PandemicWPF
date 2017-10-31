using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class Player : ObservableObject
    {
        public ObservableCollection<PlayerCard> Cards { get; private set; }

        private Pawn _pawn;
        public Pawn Pawn
        {
            get { return _pawn; }
            set
            {
                Set(ref _pawn, value);
            }
        }

        private DiseaseColor _mostCardsColor;
        public DiseaseColor MostCardsColor
        {
            get => _mostCardsColor;
        }

        public Player()
        {
            Cards = new ObservableCollection<PlayerCard>();
            Cards.CollectionChanged += Cards_CollectionChanged;
        }

        private void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Cards.Count > 0)
                _mostCardsColor =  Cards.GroupBy(x => x.City.Color).OrderByDescending(gb => gb.Count()).Select(y => y.Key).First();
            else
                _mostCardsColor = DiseaseColor.Black;
        }

        public PlayerCard RemoveCard(PlayerCard card)
        {
            this.Cards.Remove(card);
            return card;
        }

        public PlayerCard RemoveCard(City city)
        {
            var card = Cards.Single(c => c.City == city);
            return RemoveCard(card);
        }

        public void AddCard(PlayerCard card)
        {
            this.Cards.Add(card);
        }

        public int SameColorCards(Disease disease)
        {
            return this.Cards.Count(x => x.City.Color == disease.Color);
        }

        public bool HasCityCard(City city)
        {
            return Cards.Any(card => card.City == city);
        }
    }
}
