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

        public Character GetCharacter(string role, MapCity startingCity, Game game)
        {
            Character character;
            switch (role)
            {
                case Medic.MEDIC:
                    character = new Medic();
                    break;

                case OperationsExpert.OPERATIONS_EXPERT:
                    character = new OperationsExpert();
                    break;

                case Researcher.RESEARCHER:
                    character = new Researcher();
                    break;

                case ContingencyPlanner.CONTINGENCY_PLANNER:
                    character = new ContingencyPlanner();
                    break;

                case QuarantineSpecialist.QUARANTINE_SPECIALIST:
                    character = new QuarantineSpecialist();
                    break;

                case Scientist.SCIENTIST:
                    character = new Scientist();
                    break;

                default:
                    throw new ArgumentException("Unknwon role", nameof(role));
            }

            character.CurrentMapCity = startingCity;
            character.Actions = CharacterActionsFactory.GetActions(character, game);
            return character;
        }

        public IEnumerable<Character> GetCharacters(IEnumerable<string> roles, MapCity startingCity, Game game)
        {
            foreach (string role in roles)
            {
                yield return GetCharacter(role, startingCity, game);
            }
        }
    }

    public class CharacterActionsFactory
    {
        public KeyExtractorDictionary<string, IGameAction> GetActions(Character character, Game game)
        {
            switch (character.Role)
            {
                case Medic.MEDIC:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MedicMoveAction((Medic)character, game) },
                        { new TreatAction(character, game) },
                        { new BuildAction(character, game) },
                        { new ShareKnowledgeAction(character, game) },
                        { new DiscoverCureAction(character, game) }
                    };

                case OperationsExpert.OPERATIONS_EXPERT:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new OperationsExpertMoveAction((OperationsExpert)character, game) },
                        { new TreatAction(character, game) },
                        { new OperationsExpertBuildAction(character, game) },
                        { new ShareKnowledgeAction(character, game) },
                        { new DiscoverCureAction(character, game) }
                    };

                case ContingencyPlanner.CONTINGENCY_PLANNER:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character, game) },
                        { new TreatAction(character, game) },
                        { new BuildAction(character, game) },
                        { new ShareKnowledgeAction(character, game) },
                        { new DiscoverCureAction(character, game) },
                        { new ContingencyPlannerAction((ContingencyPlanner)character, game) }
                    };

                case Researcher.RESEARCHER:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character, game) },
                        { new TreatAction(character, game) },
                        { new BuildAction(character, game) },
                        { new ShareKnowledgeResearcherAction(character, game) },
                        { new DiscoverCureAction(character, game) }
                    };
                case Scientist.SCIENTIST:
                case QuarantineSpecialist.QUARANTINE_SPECIALIST:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new MoveAction(character, game) },
                        { new TreatAction(character, game) },
                        { new BuildAction(character, game) },
                        { new ShareKnowledgeAction(character, game) },
                        { new DiscoverCureAction(character, game) }
                    };

                default:
                    throw new ArgumentException("Unknwon role", nameof(character.Role));
            }
        }
    }
}