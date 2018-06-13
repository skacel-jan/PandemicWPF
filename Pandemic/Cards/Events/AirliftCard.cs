using Pandemic.GameLogic;
using System;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class AirliftCard : EventCard
    {
        public AirliftCard(CitySelectionService citySelectionService) : base("Airlift")
        {
            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
        }

        public CitySelectionService CitySelectionService { get; }

        public override void PlayEvent(Game game)
        {
            var selectCharacterAction = new Action<Character>((Character character) =>
            {               
                var selectCityAction = new Action<MapCity>((MapCity city) =>
                {
                    character.CurrentMapCity = city;

                    Task.Run(() =>
                    {
                        foreach (var mapCity in CitySelectionService.Cities)
                        {
                            mapCity.IsSelectable = false;
                        }
                    });

                    OnEventFinished(EventArgs.Empty, game);
                });

                CitySelectionService.SelectCity(CitySelectionService.Cities, selectCityAction, "Select city");
            });

            game.SelectCharacter(game.Characters, selectCharacterAction, "Select character");
        }
    }
}