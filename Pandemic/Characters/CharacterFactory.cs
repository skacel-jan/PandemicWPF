
using Pandemic.Cards;
using Pandemic.GameLogic;
using System;

namespace Pandemic.Characters
{
    public class CharacterFactory
    {
        public CharacterFactory(MapCity startingCity)
        {
            StartingCity = startingCity ?? throw new ArgumentNullException(nameof(startingCity));
        }

        public MapCity StartingCity { get; }

        public Character GetCharacter(string role)
        {
            Character character;
            switch (role)
            {
                case Medic.ROLE:
                    character = new Medic() { CurrentMapCity = StartingCity };
                    break;
                case OperationsExpert.ROLE:
                    character = new OperationsExpert() { CurrentMapCity = StartingCity };
                    break;
                case Researcher.ROLE:
                    character = new Researcher() { CurrentMapCity = StartingCity };
                    break;
                default:
                    throw new ArgumentException("Unknwon role", nameof(role));
            }

            return character;
        }
    }
}