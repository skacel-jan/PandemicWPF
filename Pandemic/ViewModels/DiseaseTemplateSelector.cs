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
            return color switch
            {
                DiseaseColor.Black => element.FindResource("BlackDisease") as DataTemplate,
                DiseaseColor.Blue => element.FindResource("BlueDisease") as DataTemplate,
                DiseaseColor.Red => element.FindResource("RedDisease") as DataTemplate,
                DiseaseColor.Yellow => element.FindResource("YellowDisease") as DataTemplate,
                _ => null,
            };
        }
    }
}