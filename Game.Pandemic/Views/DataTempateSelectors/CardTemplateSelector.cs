using System.Windows;
using System.Windows.Controls;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.Views.DataTempateSelectors
{
    internal class CardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EventCardDataTemplate { get; set; }
        public DataTemplate CityCardDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GameLogic.Cards.Card card = item as GameLogic.Cards.Card;
            if (card is EventCard)
            {
                return EventCardDataTemplate;
            }
            else
            {
                return CityCardDataTemplate;
            }
        }
    }
}