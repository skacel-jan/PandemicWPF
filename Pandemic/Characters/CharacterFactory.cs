using System;
using System.Collections.Generic;

namespace Pandemic.Characters
{
    public class CharacterFactory
    {
        public MapCity StartingCity { get; set; }

        public Character GetCharacter(string role)
        {
            Character character;
            switch (role)
            {
                case Medic.MEDIC:
                    character = new Medic() { CurrentMapCity = StartingCity };
                    break;

                case OperationsExpert.OPERATIONS_EXPERT:
                    character = new OperationsExpert() { CurrentMapCity = StartingCity };
                    break;

                case Researcher.RESEARCHER:
                    character = new Researcher() { CurrentMapCity = StartingCity };
                    break;

                case ContingencyPlanner.CONTINGENCY_PLANNER:
                    character = new ContingencyPlanner() { CurrentMapCity = StartingCity };
                    break;

                default:
                    throw new ArgumentException("Unknwon role", nameof(role));
            }

            return character;
        }

        public IEnumerable<Character> GetCharacters(IEnumerable<string> roles)
        {
            foreach (string role in roles)
            {
                yield return GetCharacter(role);
            }
        }
    }
}