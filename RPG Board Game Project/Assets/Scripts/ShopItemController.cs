using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour {
    
    private ShopController ShopController;

    public bool Sold { get; set; }
    public bool IsInInventory { get; set; }

    public CardClass Card;

    public Text TextItemName;
    public Image ImageCard;
    public Text TextPrice;
    public Button BuyButtonObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void GenerateShopItem(ShopController shopController, CardClass card, bool isInventoryItem = false)
    {
        ShopController = shopController;
        Card = card;
        IsInInventory = isInventoryItem;
        TextItemName.text = card.CardName;
        Sprite img;
        switch (card.Type)
        {
            case CardClass.CardType.ATK:
            default:
                img = Resources.Load<Sprite>("card_player/attack");
                break;
            case CardClass.CardType.GOLD:
                img = Resources.Load<Sprite>("card_player/money");
                break;
            case CardClass.CardType.POTION:
                img = Resources.Load<Sprite>("card_player/health");
                break;
        }
        ImageCard.sprite = img;
        if (IsInInventory)
        {
            TextPrice.text = Mathf.FloorToInt(card.Price * 0.3f).ToString();
            BuyButtonObject.GetComponentInChildren<Text>().text = "Sell";
        }
        else
        {
            TextPrice.text = card.Price.ToString();
            BuyButtonObject.GetComponentInChildren<Text>().text = "Add";
        }
    }

    public void ButtonClick()
    {
        switch (BuyButtonObject.GetComponentInChildren<Text>().text)
        {
            default:
                break;
            case "Add":
                ShopController.AddToCart(gameObject.transform);
                break;
            case "Remove":
                ShopController.RemoveFromCart(gameObject.transform);
                break;
            case "Sell":
                Sold = true;
                BuyButtonObject.GetComponentInChildren<Text>().text = "Cancel";
                ShopController.ItemSold(Card);
                break;
            case "Cancel":
                Sold = false;
                BuyButtonObject.GetComponentInChildren<Text>().text = "Sell";
                ShopController.ItemUnSold(Card);
                break;
        }
    }
}
