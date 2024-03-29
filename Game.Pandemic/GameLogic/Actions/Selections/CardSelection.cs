﻿using System;
using System.Collections.Generic;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic.Actions.Selections
{
    public class CardSelection : Selection
    {
        private readonly Action<Card> _selectCardCallback;
        private readonly IEnumerable<Card> _cards;
        private readonly Func<Card, bool> _predicate = null;
        
        public CardSelection(Action<Card> setCard, IEnumerable<Card> cards, string infoText)
        {
            _selectCardCallback = setCard;
            _cards = cards;
            InfoText = infoText;
        }

        public CardSelection(Action<Card> setCard, IEnumerable<Card> cards, string infoText, Func<Card, bool> predicate) : this(setCard, cards, infoText)
        {            
            _predicate = predicate;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectCard(_selectCardCallback, _cards, InfoText, _predicate);
        }
    }
}