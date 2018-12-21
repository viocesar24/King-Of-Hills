using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInventoryController : MonoBehaviour {

    public Text ItemName;
    public Image CardImage;
    public Text CardText;
    public Text ItemPrice;
    public Button ActionButton;

    private InventoryController InventoryController;

    public CardClass Card;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateInventoryItem(InventoryController controller, CardClass card)
    {
        InventoryController = controller;
        Card = card;
        ItemName.text = card.CardName;
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
        CardImage.sprite = img;
        CardText.text = "+" + card.Value + (card.Type == CardClass.CardType.GOLD ? "%" : "");
        ItemPrice.text = card.Price.ToString();
    }

    public void ButtonClick()
    {
        switch (ActionButton.GetComponentInChildren<Text>().text)
        {
            case "Equip":
                InventoryController.EquipCard(gameObject.transform);
                break;
            case "Unequip":
                InventoryController.UnequipCard(gameObject.transform);
                break;
            default:
                break;
        }
    }
}
