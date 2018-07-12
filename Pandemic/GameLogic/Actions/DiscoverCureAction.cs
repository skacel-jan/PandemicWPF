﻿using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class DiscoverCureAction : CharacterAction
    {
        public DiscoverCureAction(Character character) : base(character)
        {
        }

        public override string Name => ActionTypes.Discover;

        public override bool CanExecute(Game game)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.ColorCardsCount(Character.MostCardsColor) >= Character.CardsForCure;
        }

        protected override void Execute()
        {
            if (Character.MostCardsColorCount > Character.CardsForCure)
            {
                SelectCardsForCure();
            }
            else
            {
                DiscoverCure(Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor), Character.MostCardsColor);
            }
        }

        private void DiscoverCure(IEnumerable<CityCard> cards, DiseaseColor color)
        {
            Game.DiscoverCure(color);
            foreach (var card in new List<CityCard>(cards))
            {
                Character.RemoveCard(card);
                Game.AddCardToPlayerDiscardPile(card);
            }

            Game.Characters.OfType<Medic>().SingleOrDefault()?.SpecialTreatDisease();

            FinishAction();
        }

        private void SelectCardsForCure()
        {
            var selectedCards = new List<CityCard>();

            var action = new Action<Card>((Card card) =>
            {
                if (card is CityCard cityCard)
                {
                    if (Character.MostCardsColor == cityCard.City.Color && !selectedCards.Remove(cityCard))
                    {
                        selectedCards.Add(cityCard);
                    }
                    else if (!selectedCards.Remove(cityCard))
                    {
                        selectedCards.Add(cityCard);
                    }

                    if (Character.CardsForCure == selectedCards.Count)
                    {
                        DiscoverCure(selectedCards, Character.MostCardsColor);
                    }
                }
            });

            Game.SelectCard(Character.CityCards.Where(card => card.City.Color == Character.MostCardsColor),
                action, $"Select {Character.CardsForCure} cards of {Character.MostCardsColor} color to discover a cure");
        }
    }
}