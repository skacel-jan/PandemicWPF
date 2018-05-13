using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Pandemic.Views
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

            // Register Face dependency property
            IsSelectableProperty = DependencyProperty.Register("IsSelectable",
                typeof(bool), typeof(PlayingCard), new FrameworkPropertyMetadata(false));
        }

        public string CardName
        {
            get { return (string)GetValue(CardNameProperty); }
            set { SetValue(CardNameProperty, value); }
        }

        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        public static DependencyProperty IsSelectableProperty;
        public static DependencyProperty CardNameProperty;
    }
}
