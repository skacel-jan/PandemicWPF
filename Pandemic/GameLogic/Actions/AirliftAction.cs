using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class AirliftAction : EventAction
    {
        private Character _character;

        protected override void Execute()
        {
            _game.SelectCharacter(_game.Characters, SetCharacter, "Select character");
        }

        private void SetCity(MapCity city)
        {
            _character.CurrentMapCity = city;

            Task.Run(() =>
            {
                foreach (var mapCity in _game.WorldMap.Cities.Values)
                {
                    mapCity.IsSelectable = false;
                }
            });

            FinishAction();
        }

        private void SetCharacter(Character character)
        {
            _character = character;

            _game.SelectCity(_game.WorldMap.Cities.Values, SetCity, "Select city");
        }
    }
}