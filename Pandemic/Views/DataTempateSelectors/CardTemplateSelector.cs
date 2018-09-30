using Pandemic.Cards;
using System.Windows;
using System.Windows.Controls;

namespace Pandemic.Views
{
    internal class CardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EventCardDataTemplate { get; set; }
        public DataTemplate CityCardDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            Card card = item as Card;
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