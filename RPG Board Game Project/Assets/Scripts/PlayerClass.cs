using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    private double atk;
    private int gold;


    public bool IsMonster;

    public string Name;

    public Sprite Image;

    public int initialWaypointIndex { get; set; }

    public Vector3 RespawnPosition { get; set; }

    public double ATK {
        get
        {
            var fromEquipment = Equipped.Where(a => a.Type == CardClass.CardType.ATK).Sum(a => a.Value);
            return atk + fromEquipment;
        }
        set
        {
            atk = value;
        }
    }

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    }

    public List<CardClass> Deck { get; set; }

    public List<CardClass> Equipped { get; set; }

    public int Lives { get; set; }

    // Use this for initialization
    void Start ()
    {
        IsMonster = !(gameObject.tag == "Player");

        ATK = IsMonster ? 5.0 : 10.0;
        Gold = 500;
        Deck = new List<CardClass>();
        Equipped = new List<CardClass>();
        Lives = 5;

        // Starter Deck
        Deck.Add(new CardClass("Rusty Sword", 50, CardClass.CardType.ATK, 100));
        Deck.Add(new CardClass("Small Potion", 1, CardClass.CardType.POTION, 100));
        Deck.Add(new CardClass("Golden Medal", 10, CardClass.CardType.GOLD, 100));
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void Defeated(PlayerClass winner)
    {
        var amount = 0;
        if (!IsMonster)
        {
            amount = (int) Mathf.Floor((30f / 100f) * Gold);
            if (amount == 0)
            {
                amount = 1;
            }
            Gold -= amount;
        }
        else
        {
            amount = Gold;
        }

        if (Gold < 0)
        {
            Gold = 0;
        }

        if (winner != null)
        {
            var bonus = Mathf.FloorToInt(amount * Equipped.Where(a => a.Type == CardClass.CardType.GOLD).Sum(a => a.Value) / 100f);
            if (IsMonster)
            {
                winner.Gold += amount + (GameController.Turn * 6) + bonus;
                winner.ATK += Mathf.Ceil((30f / 100f) * (float)ATK);
            }
            else
            {
                winner.Gold += amount + bonus;
            }
        }
    }

    public void SetAsMonster(double atk, int gold)
    {
        Lives = 1;
        ATK = atk;
        Gold = gold;
        RespawnPosition = transform.position;
        Deck = new List<CardClass>(); // drops...
    }
}
