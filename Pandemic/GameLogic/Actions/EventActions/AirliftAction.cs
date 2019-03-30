using Pandemic.Cards;
using Pandemic.Decks;
using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public class AirliftAction : EventAction
    {
        private Character _selectedCharacter;

        private readonly IList<Effect> _effects;

        public AirliftAction(EventCard card, Game game, Character character) : base(card, game)
        {
            _effects = new List<Effect>
            {
                new UnselectCitiesEffect(game.WorldMap),
                new DiscardEventCardEffect(character, card, game.PlayerDiscardPile)
            };
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

    public class DiscardEventCardEffect: Effect
    {
        private readonly Character _character;
        private readonly EventCard _card;
        private readonly DiscardPile<PlayerCard> _discardpile;

        public DiscardEventCardEffect(Character character, EventCard card, DiscardPile<PlayerCard> discardpile)
        {
            _character = character;
            _card = card;
            _discardpile = discardpile;
        }

        public void Execute()
        {
            _character.RemoveCard(_card);
            _discardpile.AddCard(_card);
        }
    }

    public class CharacterSelection
    {
        private readonly Action<Character> _action;

        public CharacterSelection(Action<Character> action)
        {
            _action = action;
        }

        public void SetCharacter(Character character)
        {
            _action.Invoke(character);
        }
    }
    
    public class UnselectCitiesEffect : Effect
    {
        private readonly WorldMap _worldMap;

        public UnselectCitiesEffect(WorldMap worldMap)
        {
            _worldMap = worldMap;
        }

        public void Execute()
        {
            foreach (var mapCity in _worldMap.Cities)
            {
                mapCity.IsSelectable = false;
            }
        }
    }

    public class Effect
    {

    }
}