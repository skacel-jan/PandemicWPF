using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
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
                  new CharacterSelection(SetSelectionCallback((Character c) => _character = c),
                                         Game.Characters,
                                         "Select character")
              );

            AddSelectionState(1,
                  new CitySelection(SetSelectionCallback((MapCity c) => _city = c),
                                    Game.WorldMap.Cities,
                                    "Select city")
              );
        }
    }
}