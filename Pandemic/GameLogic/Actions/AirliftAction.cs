using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class AirliftAction : EventAction
    {
        private Character _character;

        protected override void Execute()
        {
            Game.SelectCharacter(Game.Characters, "Select character", SetCharacter);
        }

        private void SetCity(MapCity city)
        {
            _character.CurrentMapCity = city;

            FinishAction();
        }

        private void SetCharacter(Character character)
        {
            _character = character;

            Game.SelectCity(Game.WorldMap.Cities.Values, SetCity, "Select city");
        }

        protected override void FinishAction()
        {
            foreach (var mapCity in Game.WorldMap.Cities.Values)
            {
                mapCity.IsSelectable = false;
            }

            base.FinishAction();
        }
    }
}