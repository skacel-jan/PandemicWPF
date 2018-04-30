
using Pandemic.Cards;
using System;

namespace Pandemic.Characters
{
    public interface ICharacterFactory
    {
        Character GetCharacter();
    }

    public abstract class CharacterFactory : ICharacterFactory
    {
        public CharacterFactory(MapCity startingCity)
        {
            StartingCity = startingCity ?? throw new ArgumentNullException(nameof(startingCity));
        }

        public MapCity StartingCity { get; }

        public abstract Character GetCharacter();
    }

    public class MedicFactory : CharacterFactory
    {
        public MedicFactory(MapCity startingCity) : base(startingCity)
        {
        }

        public override Character GetCharacter()
        {
            var medic = new Medic()
            {
                CurrentMapCity = StartingCity
            };
            medic.ShareKnowledgeBehaviour = new ShareKnowledgeBehaviour(medic);
            medic.BuildBehaviour = new BuildBehaviour(medic);
            return medic;
        }
    }

    public class ResearcherFactory : CharacterFactory
    {
        public ResearcherFactory(MapCity startingCity) : base(startingCity)
        {
        }

        public override Character GetCharacter()
        {
            var researcher = new Researcher()
            {
                CurrentMapCity = StartingCity
            };
            researcher.ShareKnowledgeBehaviour = new ResearcherShareKnowledgeBehaviour(researcher);
            researcher.BuildBehaviour = new BuildBehaviour(researcher);
            return researcher;
        }
    }

    public class OperationsExpertFactory : CharacterFactory
    {
        public OperationsExpertFactory(MapCity startingCity) : base(startingCity)
        {
        }

        public override Character GetCharacter()
        {
            var opExpert = new OperationsExpert()
            {
                CurrentMapCity = StartingCity
            };
            opExpert.ShareKnowledgeBehaviour = new ShareKnowledgeBehaviour(opExpert);
            opExpert.BuildBehaviour = new OperationsExpertBuildBehaviour(opExpert);
            return opExpert;
        }
    }
}