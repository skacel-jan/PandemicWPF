using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;

namespace Pandemic.Characters
{
    public class CharacterFactory
    {
        public CharacterFactory(CharacterActionsFactory characterActionsFactory)
        {
            CharacterActionsFactory = characterActionsFactory ?? throw new ArgumentNullException(nameof(characterActionsFactory));
        }

        public CharacterActionsFactory CharacterActionsFactory { get; }

        public Character GetCharacter(string role, MapCity startingCity)
        {
            Character character;
            switch (role)
            {
                case Medic.MEDIC:
                    character = new Medic() { CurrentMapCity = startingCity };
                    break;

                case OperationsExpert.OPERATIONS_EXPERT:
                    character = new OperationsExpert() { CurrentMapCity = startingCity };
                    break;

                case Researcher.RESEARCHER:
                    character = new Researcher() { CurrentMapCity = startingCity };
                    break;

                case ContingencyPlanner.CONTINGENCY_PLANNER:
                    character = new ContingencyPlanner() { CurrentMapCity = startingCity };
                    break;

                case QuarantineSpecialist.QUARANTINE_SPECIALIST:
                    character = new QuarantineSpecialist() { CurrentMapCity = startingCity };
                    break;

                case Scientist.SCIENTIST:
                    character = new Scientist() { CurrentMapCity = startingCity };
                    break;

                default:
                    throw new ArgumentException("Unknwon role", nameof(role));
            }
            character.Actions = CharacterActionsFactory.GetActions(character);
            return character;
        }

        public IEnumerable<Character> GetCharacters(IEnumerable<string> roles, MapCity startingCity)
        {
            foreach (string role in roles)
            {
                yield return GetCharacter(role, startingCity);
            }
        }
    }

    public class CharacterActionsFactory
    {
        public KeyExtractorDictionary<string, IGameAction> GetActions(Character character)
        {
            switch (character.Role)
            {
                case Medic.MEDIC:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MedicMoveAction((Medic)character) },
                        { new TreatAction(character) },
                        { new BuildAction(character) },
                        { new ShareKnowledgeAction(character) },
                        { new DiscoverCureAction(character) }
                    };

                case OperationsExpert.OPERATIONS_EXPERT:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new OperationsExpertMoveAction((OperationsExpert)character) },
                        { new TreatAction(character) },
                        { new OperationsExpertBuildAction(character) },
                        { new ShareKnowledgeAction(character) },
                        { new DiscoverCureAction(character) }
                    };                

                case ContingencyPlanner.CONTINGENCY_PLANNER:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character) },
                        { new TreatAction(character) },
                        { new BuildAction(character) },
                        { new ShareKnowledgeAction(character) },
                        { new DiscoverCureAction(character) },
                        { new ContingencyPlannerAction(character) }
                    };

                case Researcher.RESEARCHER:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character) },
                        { new TreatAction(character) },
                        { new BuildAction(character) },
                        { new ShareKnowledgeResearcherAction(character) },
                        { new DiscoverCureAction(character) }
                    };
                case Scientist.SCIENTIST:
                case QuarantineSpecialist.QUARANTINE_SPECIALIST:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character) },
                        { new TreatAction(character) },
                        { new BuildAction(character) },
                        { new ShareKnowledgeAction(character) },
                        { new DiscoverCureAction(character) }
                    };

                default:
                    throw new ArgumentException("Unknwon role", nameof(character.Role));
            }
        }
    }
}