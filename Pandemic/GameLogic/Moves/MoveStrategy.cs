using Pandemic.Characters;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic
{
    public class MoveAction : CharacterAction
    {
        protected ICollection<IMoveAction> _allMoveActions;

        protected ICollection<IMoveAction> _moveActions;

        protected ICollection<IMoveAction> _moveCardActions;

        public MoveAction(Character character) : base(character)
        {
            _moveActions = new List<IMoveAction>()
            {
                new DriveOrFerry(character),
                new ShuttleFlight(character)
            };

            _moveCardActions = new List<IMoveAction>()
            {
                new DirectFlight(character),
                new CharterFlight(character)
            };

            _allMoveActions = new List<IMoveAction>(_moveActions.Concat(_moveCardActions));
        }

        protected override void Execute()
        {
            var possibleDestinationCities = GetPossibleDestinationCities(_game.WorldMap.Cities.Values);
            _game.SelectCity(possibleDestinationCities.Where(c => !c.Equals(Character.CurrentMapCity)), SelectCityCallBack, "Select destination city");
        }

        private IEnumerable<MapCity> GetPossibleDestinationCities(IEnumerable<MapCity> cities)
        {            
            foreach (var city in cities.Where(c => !c.Equals(Character.CurrentMapCity)))
            {
                if (_allMoveActions.Any(a => a.IsPossible(city)))
                {
                    yield return city;
                }
            }
        }

        public override bool CanExecute(Game game)
        {
            return true;
        }

        private void SelectCityCallBack(MapCity city)
        {
            var possibleMove = _moveActions.FirstOrDefault(x => x.IsPossible(city));
            if (possibleMove != null)
            {
                possibleMove.Move(_game, city, FinishAction);
                return;
            }

            var possibleCardMoves = _moveCardActions.Where(x => x.IsPossible(city));

            if (possibleCardMoves.Count() > 1)
            {
                var action = new Action<IMoveAction>((IMoveAction moveAction) =>
                {
                    moveAction.Move(_game, city, FinishAction);
                });

                _game.SelectMove(possibleCardMoves, action, "Select move type");
            }
            else
            {
                possibleCardMoves.First().Move(_game, city, FinishAction);
            }
        }

        protected override void FinishAction()
        {
            Task.Run(() =>
            {
                foreach (var city in _game.WorldMap.Cities.Values)
                {
                    city.IsSelectable = false;
                }
            });

            base.FinishAction();
        }
    }

    public class OperationsExpertMoveAction : MoveAction
    {
        public OperationsExpertMoveAction(Character character) : base(character)
        {
            _moveCardActions.Add(new OperationsExpertSpecialMove(character));
        }
    }

    public class MedicMoveAction : MoveAction
    {
        public MedicMoveAction(Medic medic) : base(medic)
        {
            Medic = medic;
        }

        public Medic Medic { get; }

        protected override void FinishAction()
        {
            base.FinishAction();

            Medic.SpecialTreatDisease();
        }
    }
}