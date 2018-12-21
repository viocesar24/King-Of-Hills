using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEquipController : MonoBehaviour {

    //private EquipPreviewController EquipController;

    public Text StatText;
    public Image Image;

    private CardClass Card;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateCardEquipPreview(EquipPreviewController controller, CardClass card)
    {
        //EquipController = controller;
        Card = card;
        StatText.text = "+" + Card.Value + (Card.Type == CardClass.CardType.GOLD ? "%" : "");
        StatText.color = Card.Type == CardClass.CardType.ATK ? Color.white : Color.black;
        Sprite img;
        switch (Card.Type)
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
        Image.sprite = img;
    }
}
