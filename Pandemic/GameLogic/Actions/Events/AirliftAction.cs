using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public class AirliftAction : EventAction
    {
        private Character _selectedCharacter;

        public AirliftAction(EventCard card, Game game) : base(card, game)
        {
        }

        public override void Execute()
        {
            Game.SetInfo("Select character");
            Game.SelectionService.Select(new SelectAction<Character>(SetCharacter, Game.Characters, "Select character"));
        }

        protected override void FinishAction()
        {
            foreach (var mapCity in Game.WorldMap.Cities)
            {
                mapCity.IsSelectable = false;
            }

            base.FinishAction();
        }

        private void SetCharacter(Character character)
        {
            _selectedCharacter = character;
            Game.SetInfo("Select city");
            Game.SelectionService.Select(new SelectAction<MapCity>(SetMapCity, Game.WorldMap.Cities, "Select city"));
        }

        private void SetMapCity(MapCity mapCity)
        {
            _selectedCharacter.CurrentMapCity = mapCity;
            FinishAction();
        }
    }

    public class Airlift : INext
    {
        private Queue<IAction> actions;
        private IGamePhase _phase;

        private Character _character;
        private MapCity _mapCity;

        public Game Game => _phase.Game;

        public Airlift()
        {
            actions = new Queue<IAction>();
            actions.Enqueue(new SelectAction<Character>(SetCharacter, Game.Characters, "Select character"));
            actions.Enqueue(new SelectAction<MapCity>(SetMapCity, Game.WorldMap.Cities, "Select city"));
        }

        private void SetMapCity(MapCity obj)
        {
            _mapCity = obj;
            Next();
        }

        private void SetCharacter(Character obj)
        {
            _character = obj;
            Next();
        }

        public void Execute(IGamePhase phase)
        {
            _phase = phase;
            Next();
        }

        public void Next()
        {
            if (actions.Count > 0)
            {
                var action = actions.Dequeue();
                if (action.CanExecute(null))
                {
                    action.Execute(null);
                }
                else
                {
                    Next();
                }
            }
            else
            {
                DoAction();
                _phase.End();
            }
        }

        private void DoAction()
        {
            _character.CurrentMapCity = _mapCity;
        }
    }
}