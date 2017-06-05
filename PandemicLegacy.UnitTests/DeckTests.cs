﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandemicLegacy.Decks;

namespace PandemicLegacy.UnitTests
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void ShuffleTest()
        {
            var deck = new Deck<InfectionCard>(Helper.GetInfectionCards());
            var shufledDeck = new Deck<InfectionCard>(Helper.GetInfectionCards());
            shufledDeck.Shuffle();
        }

        [TestMethod]
        public void CreatePlayerDeckTest()
        {
            var deck = new PlayerDeck(Helper.GetPlayerCards());
            deck.AddEpidemicCards(5);
            deck.Shuffle();
        }

        [TestMethod]
        public void CreateInfetionDeckTest()
        {
            var deck = new InfectionDeck(Helper.GetInfectionCards());
        }

        [TestMethod]
        public void DrawTest()
        {
            var deck = new PlayerDeck(Helper.GetPlayerCards());
            deck.AddEpidemicCards(5);
            deck.Shuffle();

            foreach (var card in deck)
            {
                System.Diagnostics.Debug.WriteLine(card);
            }

            deck.Draw();
            deck.Draw();
        }
    }
}
