using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class ShareKnowledgeAction : CharacterAction
    {
        public ShareKnowledgeAction(Character character) : base(character)
        {
        }

        public override string Name => ActionTypes.Share;

        public override bool CanExecute(Game game)
        {
            if (game.CurrentCharacter.Equals(Character))
            {
                return Character.CurrentMapCity.Characters.Count > 1 && (Character.HasCityCard(Character.CurrentMapCity.City) ||
                    Character.CurrentMapCity.Characters.Any(x => x.HasCityCard(Character.CurrentMapCity.City)));
            }
            else
            {
                return Character.HasCityCard(Character.CurrentMapCity.City);
            }
        }

        protected override void Execute()
        {
            var possibleActions = Game.Characters.Where(c => !c.Equals(Character) && c.Actions[ActionTypes.Share].CanExecute(Game))
                .Select(c => c.Actions[ActionTypes.Share]).Cast<ShareKnowledgeAction>();

            if (possibleActions.Count() > 1)
            {
                if (possibleActions.Contains(this))
                {
                    var action = new Action<ShareType>((ShareType shareType) =>
                    {
                        if (shareType == ShareType.Give)
                        {
                            SelectCharacterToGive();
                        }
                    });
                    Game.SelectShareType(Enum.GetValues(typeof(ShareType)).Cast<ShareType>(), "Select share type", action);
                }
                else
                {
                    SelectCharacterToTake(possibleActions);
                }
            }
            else
            {
                if (possibleActions.Count() == 0)
                {
                    SelectCharacterToGive();
                }
                else
                {
                    SelectCharacterToTake(possibleActions);
                }
            }
        }

        protected virtual ShareKnowledgeGive CreateGiveAction(Character characterFrom, Character characterTo)
        {
            return new ShareKnowledgeGive(characterFrom, characterTo);
        }

        protected virtual void SelectCharacterToGive()
        {
            if (Character.CurrentMapCity.Characters.Count > 2)
            {
                var action = new SelectAction<Character>(SetCharacter, Character.CurrentMapCity.Characters.Where(c => c != Character),
                    "Select character to which you will give the card");
                Game.SelectionService.Select(action);
            }
            else
            {
                CreateGiveAction(Character, Character.CurrentMapCity.Characters.Single(c => !c.Equals(Character))).Execute(Game, FinishAction);
            }

            void SetCharacter(Character character)
            {
                CreateGiveAction(Character, character).Execute(Game, FinishAction);
            }
        }

        private void SelectCharacterToTake(IEnumerable<ShareKnowledgeAction> possibleActions)
        {
            var action = new SelectAction<Character>(SetCharacter, Character.CurrentMapCity.Characters.Where(c => c != Character),
                    "Select character from which you will take card");
            Game.SelectionService.Select(action);

            void SetCharacter(Character character)
            {
                var shareAction = possibleActions.Single(a => a.Character.Equals(character));
                shareAction.CreateGiveAction(character, Character).Execute(Game, FinishAction);
            }
        }
    }

    #region Share knowledge give

    public class ShareKnowledgeGive
    {
        public ShareKnowledgeGive(Character characterFrom, Character characterTo)
        {
            CharacterFrom = characterFrom;
            CharacterTo = characterTo;
        }

        public Character CharacterFrom { get; }
        public Character CharacterTo { get; }

        public void Execute(Game game, Action callback)
        {
            game.SelectionService.Select(new SelectAction<Card>(SetCard, CharacterFrom.Cards.OfType<PlayerCard>(), "Select card to share", ValidateSelectedCard));

            void SetCard(Card card)
            {
                CharacterFrom.RemoveCard(card);
                CharacterTo.AddCard(card);

                callback();
            }
        }

        protected virtual bool ValidateSelectedCard(Card card)
        {
            return (card as PlayerCard).City.Equals(CharacterFrom.CurrentMapCity.City);
        }
    }

    #endregion Share knowledge give

    #region Researcher share knowledge

    public class ShareKnowledgeGiveResearcher : ShareKnowledgeGive
    {
        public ShareKnowledgeGiveResearcher(Character characterFrom, Character characterTo) : base(characterFrom, characterTo)
        {
        }

        protected override bool ValidateSelectedCard(Card card)
        {
            return true;
        }
    }

    public class ShareKnowledgeResearcherAction : ShareKnowledgeAction
    {
        public ShareKnowledgeResearcherAction(Character character) : base(character)
        {
        }

        public override bool CanExecute(Game game)
        {
            if (game.CurrentCharacter.Equals(Character))
            {
                return Character.CurrentMapCity.Characters.Count > 1 && Character.Cards.Count > 0;
            }
            else
            {
                return Character.Cards.Count > 0;
            }
        }

        protected override ShareKnowledgeGive CreateGiveAction(Character from, Character to)
        {
            return new ShareKnowledgeGiveResearcher(from, to);
        }
    }

    #endregion Researcher share knowledge
}