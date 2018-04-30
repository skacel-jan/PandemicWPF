using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pandemic.Cards
{
    public interface IAction
    {
        void Execute(object parameter);
    }

    public class BuildResearchStationCommand : IAction
    {
        private CitySelectedCommand _selectCityCommand;

        public BuildResearchStationCommand(CitySelectedCommand selectCityCommand)
        {
            _selectCityCommand = selectCityCommand ?? throw new ArgumentNullException(nameof(selectCityCommand));
        }

        public void Execute(object parameter)
        {
            _selectCityCommand.Execute(parameter);
            _selectCityCommand.City.HasResearchStation = true;
        }
    }

    public class CitySelectedCommand : IAction
    {
        public MapCity City { get; private set; }

        public void Execute(object parameter)
        {
            City = parameter as MapCity;
        }
    }

    public class EventCard : Card
    {
        public EventCard(string name) : base(name)
        {
            using (var memory = new MemoryStream())
            {
                Properties.Resources.VirusWhite.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                Image = bitmapImage;
            }
        }     
        
        public Character Character { get; set; }

        public string EventCode { get => ActionTypes.GovernmentGrant; }
    }
}