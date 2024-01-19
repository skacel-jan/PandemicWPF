using System.Collections.Generic;
using System.Linq;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Moves;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions
{
    public class DispatchAction : CharacterAction
    {
        private CityCard _card;
        private MapCity _city;
        private Character _character;

        public IEnumerable<IMoveAction> AllMoveActions => MoveActions.Concat(CardMoveActions);
        public override string Name => ActionTypes.Dispatch;
        protected ICollection<ICardMoveAction> CardMoveActions { get; }
        protected ICollection<IMoveAction> MoveActions { get; }
        protected IMoveAction SelectedMoveAction { get; set; }
        protected ICardMoveAction SelectedCardMoveAction { get; set; }

        public DispatchAction(Character character, Game game) : base(character, game)
        {
            MoveActions = new List<IMoveAction>()
            {
                new DriveOrFerry(character),
                new ShuttleFlight(character),
                new DispatcherSpecialMove(character)
            };

            CardMoveActions = new List<ICardMoveAction>()
            {
                new DirectFlight(character),
                new CharterFlight(character)
            };
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            foreach (var effect in _character.GetMoveEffects(_city, Game))
            {
                Effects.Add(effect);
            }

            Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));

            if (_card != null)
            {
                Effects.Add(new DiscardPlayerCardEffect(_card, Game.PlayerDiscardPile));
            }
        }

        protected override void Initialize()
        {
            AddSelectionState(0,                
                new CharacterSelection(
                    SelectionCallback<Character>((c) => _character = c),
                    Game.Characters.Where(c => !c.Equals(Character)),
                    "Select character which you want to dispatch")
                );

            AddSelectionState(1,
                new CitySelection(
                    SelectionCallback<MapCity>((c) => _city = c),
                    GetPossibleDestinationCities(Game.WorldMap.Cities),
                    "Select city"));

            AddContinueState(2,
                (g) => MoveActions.Any(x => x.IsPossible(_city)),
                (g) => SelectedMoveAction = MoveActions.First(x => x.IsPossible(_city))
                );

            AddSelectionState(2,
                (g) => SelectedMoveAction == null && CardMoveActions.Count(x => x.IsPossible(_city)) > 1,
                new MoveSelection(SelectionCallback((IMoveAction move) => SelectedCardMoveAction = (ICardMoveAction)move),
                            CardMoveActions.Where(x => x.IsPossible(_city)), "Select move"));

            AddContinueState(2,
                (g) => SelectedMoveAction == null && CardMoveActions.Count(x => x.IsPossible(_city)) == 1,
                (g) => SelectedCardMoveAction = CardMoveActions.First(x => x.IsPossible(_city))
                );

            AddSelectionState(3,
                (g) => SelectedCardMoveAction != null,
                   new CardSelection(
                       SelectionCallback((Card card) => _card = (CityCard)card),
                       Character.CityCards,
                       "Select city card",
                       (Card card) => SelectedCardMoveAction.Validate(_city, (CityCard)card))
                );
        }

        private IEnumerable<MapCity> GetPossibleDestinationCities(IEnumerable<MapCity> cities)
        {
            foreach (var city in cities.Where(c => !c.Equals(Character.CurrentMapCity)))
            {
                if (AllMoveActions.Any(a => a.IsPossible(city)))
                {
                    yield return city;
                }
            }
        }
    }
}