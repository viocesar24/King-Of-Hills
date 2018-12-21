using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public static class CardProvider
    {
        public static CardClass[] GetShopCards(string townName)
        {
            switch (townName)
            {
                case "Magnolia":
                default:
                    return new CardClass[]
                    {
                         // Sword
                         new CardClass("Rusty Sword", 28, CardClass.CardType.ATK, 200),
                         new CardClass("Bronze Sword", 71, CardClass.CardType.ATK, 450),
                         new CardClass("Iron Sword", 142, CardClass.CardType.ATK, 980),
                         new CardClass("Platinum Sword", 311, CardClass.CardType.ATK, 2100),
                         new CardClass("Divine Sword", 1000, CardClass.CardType.ATK, 5000),

                         //Gold Booster
                         new CardClass("Gold Booster I", 20, CardClass.CardType.GOLD, 650),
                         new CardClass("Gold Booster II", 40, CardClass.CardType.GOLD, 1000),

                         //Potion
                         new CardClass("Small Potion", 1, CardClass.CardType.POTION, 500),
                         new CardClass("Medium Potion", 3, CardClass.CardType.POTION, 1000),
                         new CardClass("Elixir", 5, CardClass.CardType.POTION, 3200)
                    };
            }
        }
    }
}
