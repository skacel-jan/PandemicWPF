using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class MoveAction : CharacterAction
    {
        private MapCity _city;
        private CityCard _card;
        protected IMoveAction SelectedMoveAction { get; private set; }

        public MoveAction(Character character, Game game) : base(character, game)
        {
            MoveActions = new List<IMoveAction>()
            {
                new DriveOrFerry(character),
                new ShuttleFlight(character)
            };

            CardMoveActions = new List<ICardMoveAction>()
            {
                new DirectFlight(character),
                new CharterFlight(character)
            };
        }

        public override string Name => ActionTypes.Move;

        public IEnumerable<IMoveAction> AllMoveActions => MoveActions.Concat(CardMoveActions);

        protected ICollection<IMoveAction> MoveActions { get; }

        protected ICollection<ICardMoveAction> CardMoveActions { get; }

        protected override void AddEffects()
        {
            Effects.Add(new MoveCharacterToCityEffect(Character, _city));
            Effects.Add(new UnselectAllCitiesEffect(Game.WorldMap));

            if (_card != null)
            {
                Effects.Add(new DiscardPlayerCardEffect(_card, Game.PlayerDiscardPile));
            }         
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            yield return new CitySelection(GetCitySelectCallback(), GetPossibleDestinationCities(Game.WorldMap.Cities), "Select city");
        }

        private Action<MapCity> GetCitySelectCallback()
        {
            var action = new Action<MapCity>((MapCity c) =>
            {
                _city = c;
                var possibleMoveActions = MoveActions.FirstOrDefault(x => x.IsPossible(_city));
                if (possibleMoveActions != null)
                {
                    SelectedMoveAction = possibleMoveActions;
                }

                var possibleCardMoveActions = CardMoveActions.Where(x => x.IsPossible(_city)).ToArray();
                ICardMoveAction selectedMoveAction = null;
                if (possibleCardMoveActions.Length > 1)
                {
                    Selections.Enqueue(new MoveSelection(SetSelectionCallback((IMoveAction move) => selectedMoveAction = (ICardMoveAction)move),
                        possibleCardMoveActions, "Select move"));
                }
                else
                {
                    selectedMoveAction = possibleCardMoveActions.First();
                }

                Selections.Enqueue(new CardSelection(SetSelectionCallback((Card card) => _card = (CityCard)card), Character.CityCards,
                        "Select city card", (Card card) => selectedMoveAction.Validate(_city, (CityCard)card)));

                Execute();
            });

            return action;
        }

        private IEnumerable<MapCity> GetPossibleDestinationCities(IEnumerable<MapCity> cities)
        {
            foreach (var city in cities.Where(c => !c.Equals(Character.CurrentMapCity)))
            {
                if (AllMoveActions.Any(a => a.IsPossible(city)))
                {
                    yield return city;
                }
            }
        }
    }

    public class MedicMoveAction : MoveAction
    {
        public MedicMoveAction(Medic character, Game game) : base(character, game)
        {
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            foreach (var disease in Character.CurrentMapCity.Diseases.Values)
            {
                if (disease.Status > Disease.State.NotCured)
                {
                    Effects.Add(new TreatDiseaseEffect(Game, disease.Color, Character));
                }
            }
        }
    }

    public class OperationsExpertMoveAction : MoveAction
    {
        private readonly OperationsExpertSpecialMove _operationsExpertSpecialMove;
        private int _specialMovePlayedTurn = 0;

        public OperationsExpertMoveAction(OperationsExpert character, Game game) : base(character, game)
        {
            _operationsExpertSpecialMove = new OperationsExpertSpecialMove(character, game);
            CardMoveActions.Add(_operationsExpertSpecialMove);
        }

        protected override void AddEffects()
        {
            base.AddEffects();

            if (SelectedMoveAction == _operationsExpertSpecialMove)
            {
                _specialMovePlayedTurn = Game.Turn;
            }
        }        

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            if (_specialMovePlayedTurn == Game.Turn)
            {
                CardMoveActions.Remove(_operationsExpertSpecialMove);
            }
            else if (!CardMoveActions.Contains(_operationsExpertSpecialMove))
            {
                CardMoveActions.Add(_operationsExpertSpecialMove);
            }

            return base.PrepareSelections(game);
        }
    }
}