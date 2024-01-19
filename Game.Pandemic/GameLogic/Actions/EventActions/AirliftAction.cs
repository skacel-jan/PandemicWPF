using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.EventActions
{
    public class AirliftAction : EventAction
    {
        private MapCity _city;
        private Character _character;

        public AirliftAction(EventCard card, Game game) : base(card, game)
        {
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));
            Effects.Add(new MoveCharacterToCityEffect(_character, _city));
        }

        protected override void Initialize()
        {
            AddSelectionState(0,
                  new CharacterSelection(SelectionCallback((Character c) => _character = c),
                                         Game.Characters,
                                         "Select character")
              );

            AddSelectionState(1,
                  new CitySelection(SelectionCallback((MapCity c) => _city = c),
                                    Game.WorldMap.Cities,
                                    "Select city")
              );
        }
    }
}