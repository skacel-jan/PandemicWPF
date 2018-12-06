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
}