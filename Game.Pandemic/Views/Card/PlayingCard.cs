using System.Windows;
using System.Windows.Controls.Primitives;

namespace Game.Pandemic.Views.Card
{
    public class PlayingCard : ToggleButton
    {
        static PlayingCard()
        {
            // Override style
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlayingCard),
                new FrameworkPropertyMetadata(typeof(PlayingCard)));

            // Register CardName dependency property
            CardNameProperty = DependencyProperty.Register("CardName",
                typeof(string), typeof(PlayingCard));
        }

        public string CardName
        {
            get { return (string)GetValue(CardNameProperty); }
            set { SetValue(CardNameProperty, value); }
        }

        public static DependencyProperty CardNameProperty;
    }
}