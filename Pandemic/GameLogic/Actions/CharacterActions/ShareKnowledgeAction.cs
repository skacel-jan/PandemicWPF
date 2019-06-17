using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class ShareKnowledgeAction : CharacterAction
    {
        private Character _characterFrom;
        private Character _characterTo;
        private CityCard _card;
        private ShareType _shareType;

        public ShareKnowledgeAction(Character character, Game game) : base(character, game)
        {

        }

        public override string Name => ActionTypes.Share;

        public override bool CanExecute()
        {
            if (Game.CurrentCharacter.Equals(Character))
            {
                return Character.CurrentMapCity.Characters.Count > 1 && (Character.HasCityCard(Character.CurrentMapCity.City) ||
                    Character.CurrentMapCity.Characters.Any(x => x.HasCityCard(Character.CurrentMapCity.City)));
            }
            else
            {
                return CanGiveCard();
            }
        }

        public virtual bool CanGiveCard()
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        private Selection GetCardSelection(Character character)
        {
            return new CardSelection(SetSelectionCallback((Card c) => _card = (CityCard)c), character.CityCards, "Select card",
                (card) => ValidateCard(card));
        }

        private void SetCharacters(Character characterFrom, Character characterTo)
        {
            _characterFrom = characterFrom;
            _characterTo = characterTo;
        }

        protected virtual bool ValidateCard(Card card)
        {
            return (card as CityCard).City.Equals(Character.CurrentMapCity.City);
        }

        private Selection GetOtherCharacterCardSelection(Character character)
        {
            Func<Card, bool> action = ((ShareKnowledgeAction)character.Actions[ActionTypes.Share]).ValidateCard;

            return new CardSelection(SetSelectionCallback((Card c) => _card = (CityCard)c), character.CityCards,
            $"Select card from {character}", action);
        }

        protected override void AddEffects()
        {
            base.AddEffects();
            Effects.Add(new GiveCardEffect(_characterFrom, _characterTo, _card));
        }

        protected override void Initialize()
        {
            var possibleActions = Game.Characters.Where(c => c.CurrentMapCity == Character.CurrentMapCity)
                .Select(c => (ShareKnowledgeAction)c.Actions[ActionTypes.Share])
                 .Where(c => c.CanGiveCard()).ToList();

            AddSelectionState(0,
                (g) => possibleActions.Count > 1 && possibleActions.Contains(this),
                    new ShareTypeSelection(SetSelectionCallback<ShareType>((s) => _shareType = s), Enum.GetValues(typeof(ShareType)).Cast<ShareType>(), "Select share type")
                );

            AddContinueState(0,
                (g) => possibleActions.Count > 0 && !possibleActions.Contains(this),
                (g) => _shareType = ShareType.Take
                );

            AddContinueState(0,
                (g) => possibleActions.Count == 0,
                (g) => _shareType = ShareType.Give
                );

            AddSelectionState(1,
                (g) => _shareType == ShareType.Give && Character.CurrentMapCity.Characters.Count(c => !c.Equals(Character)) > 1,
                new CharacterSelection(SetSelectionCallback<Character>((c) => SetCharacters(Character, c)),
                                       Character.CurrentMapCity.Characters,
                                       "Select character to whom you will give the card")
                );

            AddContinueState(1,
               (g) => _shareType == ShareType.Give && Character.CurrentMapCity.Characters.Count(c => !c.Equals(Character)) == 1,
               (g) => SetCharacters(Character, Character.CurrentMapCity.Characters.First(c => !c.Equals(Character)))
               );

            AddSelectionState(1,
                (g) => _shareType == ShareType.Take && possibleActions.Count(c => !c.Character.Equals(Character)) > 1,
                 new CharacterSelection(SetSelectionCallback<Character>((c) => SetCharacters(c, Character)),
                                        possibleActions.Where(c => !c.Character.Equals(Character)).Select(c => c.Character),
                                        "Select character from which you will take card")
                );

            AddContinueState(1,
               (g) => _shareType == ShareType.Take && possibleActions.Count(c => !c.Character.Equals(Character)) == 1,
               (g) => SetCharacters(possibleActions.Where(c => !c.Character.Equals(Character)).Select(c => c.Character).First(), Character)
               );

            AddSelectionState(2,
                (g) => true,
                () => _shareType == ShareType.Give ? GetCardSelection(_characterFrom) : GetOtherCharacterCardSelection(_characterFrom)
                );
        }
    }

    public class ShareKnowledgeResearcherAction : ShareKnowledgeAction
    {
        public ShareKnowledgeResearcherAction(Character character, Game game) : base(character, game)
        {
        }

        public override bool CanExecute()
        {
            if (Game.CurrentCharacter.Equals(Character))
            {
                return Character.CurrentMapCity.Characters.Count > 1 && Character.Cards.Count > 0;
            }
            else
            {
                return CanGiveCard();
            }
        }

        public override bool CanGiveCard()
        {
            return Character.Cards.Count > 0;
        }

        protected override bool ValidateCard(Card card) => true;
    }
}