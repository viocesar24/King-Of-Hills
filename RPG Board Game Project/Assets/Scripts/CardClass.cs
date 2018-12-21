using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClass {
    public enum CardType
    {
        ATK, GOLD, POTION
    }

    public string CardName;
    public string CardDescription;
    public int Value;
    public CardType Type; // 0: ATK | 1: Gold Boost | 2: Potion
    public bool Consumable;
    public int Price;

    public CardClass(string cardName, int value, CardType type, int price, bool consumable = false)
    {
        CardName = cardName;
        Value = value;
        Type = type;
        Consumable = consumable;
        Price = price;

        switch (Type)
        {
            case CardType.GOLD:
                CardDescription = "+" + Value + "% gold gain";
                break;
            case CardType.POTION:
                CardDescription = Value < 5 ? "Restore " + Value + " health" : "Fully restore health";
                Consumable = true;
                break;
            case CardType.ATK:
                CardDescription = "+" + Value + " ATK";
                break;
            default:
                break;
        }
    }
}
