using Pandemic.Cards;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using Pandemic.ViewModels;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class SelectionService
    {
        public SelectionService(WorldMap worldMap)
        {
            WorldMap = worldMap ?? throw new ArgumentNullException(nameof(worldMap));
        }

        public WorldMap WorldMap { get; }

        internal void SelectCharacter(Action<Character> selectCharacterCallback, IEnumerable<Character> characters, string text)
        {
            var viewModel = new CharacterSelectionViewModel(GetCallback(selectCharacterCallback), characters);
            BoardViewModel.ActionViewModel = viewModel;
            BoardViewModel.Game.SetInfo(text);
        }

        internal void SelectCards(Action<IEnumerable<Card>> selectCardsCallback, IEnumerable<Card> cards, string infoText, 
            Func<IEnumerable<Card>, bool> predicate)
        {
            var viewModel = new CardsSelectionViewModel(GetCallback(selectCardsCallback), cards, predicate);
            BoardViewModel.ActionViewModel = viewModel;
            BoardViewModel.Game.SetInfo(infoText);
        }

        internal void SelectShareType(Action<ShareType> action, IEnumerable<ShareType> items, string infoText)
        {
            BoardViewModel.ActionViewModel = new ShareTypeSelectionViewModel(GetCallback(action), items);
            BoardViewModel.Game.SetInfo(infoText);
        }

        private Action<T> GetCallback<T>(Action<T> callback)
        {
            return (t) =>
            {
                BoardViewModel.ActionViewModel = null;
                BoardViewModel.Game.Info = null;
                callback(t);
                BoardViewModel.RefreshAllCommands();
            };
        }

        internal void SelectCity(Action<MapCity> selectCityCallback, IEnumerable<MapCity> cities, string text)
        {
            BoardViewModel.Game.SetInfo(text);
            foreach (var city in cities)
            {
                city.IsSelectable = true;
            }

            WorldMap.SelectCity(GetCallback(selectCityCallback));
        }

        internal void SelectDisease(Action<DiseaseColor> action, IList<DiseaseColor> diseasesToTreat, string infoText)
        {
            var viewModel = new DiseaseSelectionViewModel(GetCallback(action), diseasesToTreat);
            BoardViewModel.ActionViewModel = viewModel;
            BoardViewModel.Game.SetInfo(infoText);
        }

        public BoardViewModel BoardViewModel { get; internal set; }

        internal void SelectCard(Action<Card> selectCardCallback, IEnumerable<Card> cards, string infoText)
        {
            SelectCard(selectCardCallback, cards, infoText, (Card c) => true);
        }

        internal void SelectCard(Action<Card> selectCardCallback, IEnumerable<Card> cards, string infoText, Func<Card, bool> predicate)
        {
            var viewModel = new CardSelectionViewModel(GetCallback(selectCardCallback), cards, predicate);
            BoardViewModel.ActionViewModel = viewModel;
            BoardViewModel.Game.SetInfo(infoText);
        }

        internal void SelectMove(Action<IMoveAction> selectActionCallback, IEnumerable<IMoveAction> possibleCardMoveActions, string infoText)
        {
            var viewModel = new MoveSelectionViewModel(GetCallback(selectActionCallback), possibleCardMoveActions);
            BoardViewModel.ActionViewModel = viewModel;
            BoardViewModel.Game.SetInfo(infoText);
        }
    }
}