using Pandemic.GameLogic;
using System.Windows;
using System.Windows.Controls;

namespace Pandemic.ViewModels
{
    public class DiseaseTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            DiseaseColor color = (DiseaseColor)item;
            switch (color)
            {
                case DiseaseColor.Black:
                    return element.FindResource("BlackDisease") as DataTemplate;

                case DiseaseColor.Blue:
                    return element.FindResource("BlueDisease") as DataTemplate;

                case DiseaseColor.Red:
                    return element.FindResource("RedDisease") as DataTemplate;

                case DiseaseColor.Yellow:
                    return element.FindResource("YellowDisease") as DataTemplate;

                default:
                    return null;
            }
        }
    }
}