using System.Linq;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Actions.Selections;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.CharacterActions.SpecialCharacterActions
{
    public class ContingencyPlannerAction : CharacterAction
    {
        private readonly ContingencyPlanner _contingancyPlanner;
        private EventCard _eventCard;

        public ContingencyPlannerAction(ContingencyPlanner contingancyPlanner, Game game) : base(contingancyPlanner, game)
        {
            _contingancyPlanner = contingancyPlanner;
        }

        public override string Name => "Special";

        public override bool CanExecute()
        {
            return Game.PlayerDiscardPile.Cards.Any(c => c is EventCard);
        }

        protected override void AddEffects()
        {
            base.AddEffects();
            Effects.Add(new ReturnEventEffect(_eventCard, Game, _contingancyPlanner));
        }

        protected override void Initialize()
        {
            AddSelectionState(0, 
                new CardSelection(SelectionCallback((Card c) => _eventCard = (EventCard)c), 
                                  Game.PlayerDiscardPile.Cards.OfType<EventCard>(),
                                  "Select event card to save"));
        }
    }
}