using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic.Decks;

namespace Pandemic.UnitTests
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
            var deck = new PlayerDeck(Helper.GetCities());
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
            var deck = new PlayerDeck(Helper.GetCities());
            deck.AddEpidemicCards(5);
            deck.Shuffle();

            foreach (var card in deck.Cards)
            {
                System.Diagnostics.Debug.WriteLine(card);
            }

            deck.Draw();
            deck.Draw();
        }
    }
}
