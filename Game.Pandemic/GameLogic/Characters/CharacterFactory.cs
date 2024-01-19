using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Actions;
using Game.Pandemic.GameLogic.Actions.CharacterActions;
using Game.Pandemic.GameLogic.Actions.CharacterActions.SpecialCharacterActions;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.Helpers;

namespace Game.Pandemic.GameLogic.Characters
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
            Character character = role switch
            {
                Medic.MEDIC => new Medic(),
                OperationsExpert.OPERATIONS_EXPERT => new OperationsExpert(),
                Researcher.RESEARCHER => new Researcher(),
                ContingencyPlanner.CONTINGENCY_PLANNER => new ContingencyPlanner(),
                QuarantineSpecialist.QUARANTINE_SPECIALIST => new QuarantineSpecialist(),
                Scientist.SCIENTIST => new Scientist(),
                Dispatcher.DISPATCHER => new Dispatcher(),
                _ => throw new ArgumentException("Unknwon role", nameof(role)),
            };
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
                        { new MoveAction(character, game) },
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
                case Dispatcher.DISPATCHER:
                    return new KeyExtractorDictionary<string, IGameAction>((IGameAction a) => a.Name)
                    {
                        { new DispatcherMoveAction(character, game) },
                        { new TreatAction(character, game) },
                        { new BuildAction(character, game) },
                        { new ShareKnowledgeAction(character, game) },
                        { new DiscoverCureAction(character, game) },
                        { new DispatchAction(character, game) }
                    };

                default:
                    throw new ArgumentException("Unknwon role", nameof(character.Role));
            }
        }
    }
}