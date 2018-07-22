using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
