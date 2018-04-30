using System;

namespace Pandemic
{
    public class ResearcherShareKnowledgeBehaviour : ShareKnowledgeBehaviour
    {
        public ResearcherShareKnowledgeBehaviour(Character character) : base(character)
        {
        }

        public override bool CanShare(PlayerCard cityCard)
        {
            return Character.HasCityCard(cityCard.City);
        }

        public override bool IsPossible()
        {
            return Character.Cards.Count > 0;
        }

        public override void Share(Character toCharacter, PlayerCard card)
        {
            Character.RemoveCard(card);
            toCharacter.AddCard(card);
        }
    }

    public class ShareKnowledgeBehaviour
    {
        public ShareKnowledgeBehaviour(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public Character Character { get; }

        public virtual bool CanShare(PlayerCard cityCard)
        {
            return Character.HasCityCard(cityCard.City) && cityCard.City == Character.CurrentMapCity.City;
        }

        public virtual bool IsPossible()
        {
            return Character.Cards.Count > 0 && Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public virtual void Share(Character toCharacter, PlayerCard card)
        {
            Character.RemoveCard(card);
            toCharacter.AddCard(card);
        }
    }
}