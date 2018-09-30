using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class MoveAction : CharacterAction
    {
        public IEnumerable<IMoveAction> AllMoveActions => MoveActions.Concat(MoveCardActions);

        protected ICollection<IMoveAction> MoveActions { get; }

        protected ICollection<IMoveAction> MoveCardActions { get; }

        public MoveAction(Character character) : base(character)
        {
            MoveActions = new List<IMoveAction>()
            {
                new DriveOrFerry(character),
                new ShuttleFlight(character)
            };

            MoveCardActions = new List<IMoveAction>()
            {
                new DirectFlight(character),
                new CharterFlight(character)
            };
        }

        public override string Name => ActionTypes.Move;

        public override bool CanExecute(Game game)
        {
            return true;
        }

        protected override void Execute()
        {
            var possibleDestinationCities = GetPossibleDestinationCities(Game.WorldMap.Cities);

            Game.SelectionService.Select(new SelectAction<MapCity>(SetCity, GetPossibleDestinationCities(Game.WorldMap.Cities), "Select destination city"));

            void SetCity(MapCity mapCity)
            {
                MoveToCity(mapCity);
            }
        }

        protected override void FinishAction()
        {
            foreach (var city in Game.WorldMap.Cities)
            {
                city.IsSelectable = false;
            }

            base.FinishAction();
        }

        private IEnumerable<MapCity> GetPossibleDestinationCities(IEnumerable<MapCity> cities)
        {
            foreach (var city in cities.Where(c => !c.Equals(Character.CurrentMapCity)))
            {
                if (AllMoveActions.Any(a => a.IsPossible(Game, city)))
                {
                    yield return city;
                }
            }
        }

        private void MoveToCity(MapCity city)
        {
            var possibleMove = MoveActions.FirstOrDefault(x => x.IsPossible(Game, city));
            if (possibleMove != null)
            {
                possibleMove.Move(Game, city, FinishAction);
                return;
            }

            var possibleCardMoves = MoveCardActions.Where(x => x.IsPossible(Game, city));

            if (possibleCardMoves.Count() > 1)
            {
                var action = new Action<IMoveAction>((IMoveAction moveAction) =>
                {
                    moveAction.Move(Game, city, FinishAction);
                });

                Game.SelectMove(possibleCardMoves, "Select move type", action);
            }
            else
            {
                possibleCardMoves.FirstOrDefault()?.Move(Game, city, FinishAction);
            }
        }
    }

    #region Special move actions

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

    public class OperationsExpertMoveAction : MoveAction
    {
        public OperationsExpertMoveAction(OperationsExpert character) : base(character)
        {
            MoveCardActions.Add(new OperationsExpertSpecialMove(character));
        }
    }

    #endregion Special move actions
}