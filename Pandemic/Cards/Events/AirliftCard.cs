using Pandemic.GameLogic;
using System;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class AirliftCard : EventCard
    {
        public AirliftCard(CharacterSelectionService characterSelectionService, CitySelectionService citySelectionService) : base("Airlift")
        {
            CharacterSelectionService = characterSelectionService ?? throw new ArgumentNullException(nameof(characterSelectionService));
            CitySelectionService = citySelectionService ?? throw new ArgumentNullException(nameof(citySelectionService));
        }

        public CitySelectionService CitySelectionService { get; }
        public CharacterSelectionService CharacterSelectionService { get; }

        public override void PlayEvent()
        {
            var selectCharacterAction = new Action<Character>((Character character) =>
            {
                Task.Run(() =>
                {
                    foreach (var city in CitySelectionService.Cities)
                    {
                        city.IsSelectable = true;
                    }
                });

                var selectCityAction = new Action<MapCity>((MapCity city) =>
                {
                    character.CurrentMapCity = city;

                    Character.RemoveCard(this);

                    Task.Run(() =>
                    {
                        foreach (var mapCity in CitySelectionService.Cities)
                        {
                            mapCity.IsSelectable = false;
                        }
                    });

                    OnEventFinished(EventArgs.Empty);
                });


                CitySelectionService.SelectCity("Select city", selectCityAction);
            });

            CharacterSelectionService.SelectCharacter("Select character", CharacterSelectionService.Characters, selectCharacterAction);
        }
    }
}