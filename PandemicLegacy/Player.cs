using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
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

        public Player()
        {
            Cards = new ObservableCollection<PlayerCard>();
        }

        public void RemoveCard(PlayerCard card)
        {
            this.Cards.Remove(card);
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

        public void RemoveCardWithCity(City city)
        {
            RemoveCard(Cards.Single(c => c.City == city));
        }
    }
}
