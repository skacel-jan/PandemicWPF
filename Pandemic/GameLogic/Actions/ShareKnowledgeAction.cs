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
                return Character.HasCityCard(Character.CurrentMapCity.City);
            }
        }

        protected virtual Selection SelectCharacterToGive()
        {
            var otherCharacters = Character.CurrentMapCity.Characters.Where(c => !c.Equals(Character));

            if (otherCharacters.Count() > 1)
            {
                return new CharacterSelection(SetSelectionCallback<Character>((c) =>
                {
                    SetCharacter(c);
                    Selections.Enqueue(GetCardSelection(c));
                }), otherCharacters,
                    "Select character to whom you will give the card");
            }
            else
            {
                SetCharacter(otherCharacters.First());

                return GetCardSelection(otherCharacters.First());
            }

            void SetCharacter(Character character)
            {
                _characterTo = character;
                _characterFrom = Character;
            }

            Selection GetCardSelection(Character character)
            {
                return new CardSelection(SetSelectionCallback((Card c) => _card = (CityCard)c), _characterFrom.CityCards, "Select card",
                    (card) => ValidateCard(card));
            }
        }

        protected virtual bool ValidateCard(Card card)
        {
            return (card as CityCard).City.Equals(Character.CurrentMapCity.City);
        }

        private Selection SelectCharacterToTake(IEnumerable<ShareKnowledgeAction> possibleActions)
        {
            var otherCharacters = possibleActions.Where(c => !c.Character.Equals(Character)).Select(x => x.Character);

            if (otherCharacters.Count() > 1)
            {
                return new CharacterSelection(SetSelectionCallback<Character>((c) =>
                {
                    SetCharacter(c);
                    Selections.Enqueue(GetOtherCharacterCardSelection(c));
                }), otherCharacters,
                    "Select character from which you will take card");
            }
            else
            {
                SetCharacter(otherCharacters.First());

                return GetOtherCharacterCardSelection(otherCharacters.First());
            }

            void SetCharacter(Character character)
            {
                _characterTo = Character;
                _characterFrom = character;
            }

            Selection GetOtherCharacterCardSelection(Character character)
            {
                Func<Card, bool> action = ((ShareKnowledgeAction)character.Actions[ActionTypes.Share]).ValidateCard;

                return new CardSelection(SetSelectionCallback((Card c) => _card = (CityCard)c), character.CityCards,
                $"Select card from {character}", action);
            }
        }

        protected override void AddEffects()
        {
            Effects.Add(new GiveCardEffect(_characterFrom, _characterTo, _card));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            var possibleActions = Game.Characters.Select(c => (ShareKnowledgeAction)c.Actions[ActionTypes.Share])
                .Where(c => !c.Character.Equals(Character) && c.CanExecute()).ToList();

            if (possibleActions.Count > 1)
            {
                if (possibleActions.Contains(this))
                {
                    var action = new Action<ShareType>((shareType) =>
                    {
                        if (shareType == ShareType.Give)
                        {
                            Selections.Enqueue(SelectCharacterToGive());
                        }
                    });

                    yield return new ShareTypeSelection(SetSelectionCallback(action), Enum.GetValues(typeof(ShareType)).Cast<ShareType>(), "Select share type");
                }
                else
                {
                    yield return SelectCharacterToTake(possibleActions);
                }
            }
            else
            {
                if (possibleActions.Count == 0)
                {
                    yield return SelectCharacterToGive();
                }
                else
                {
                    yield return SelectCharacterToTake(possibleActions);
                }
            }
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
                return Character.Cards.Count > 0;
            }
        }

        protected override bool ValidateCard(Card card) => true;
    }
}