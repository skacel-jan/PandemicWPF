using Pandemic.Cards;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public class AirliftAction : EventAction
    {
        private Character _character;
        private MapCity _city;

        public AirliftAction(EventCard card, Game game) : base(card, game)
        {

        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            return new Selection[]
            {
                new CharacterSelection(SetSelectionCallback((Character c) => _character = c), game.Characters, "Select character"),
                new CitySelection(SetSelectionCallback((MapCity c) => _city = c), game.WorldMap.Cities, "Select city")
            };
        }

        protected override void AddEffects()
        {
            Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));
            Effects.Add(new MoveCharacterToCityEffect(_character, _city));
        }
    }
}