using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    [SerializeField]
    private Transform prefab_ShopItem;
    [SerializeField]
    private CartController CartController;
    [SerializeField]
    private ShopInventoryController ShopInventoryController;
    [SerializeField]
    private Text SumOfCostsText;
    [SerializeField]
    private Button ConfirmButton;

    private int SumOfCosts;

    public static string LastTownName = "";

    public GameObject GridLayoutGroup_ShopItems;
    public GameObject GridLayoutGroup_Inventory;

    private PlayerClass ActivePlayer;
    private RectTransform rect;
    private List<CardClass> ItemSoldList;

    private List<ShopItemController> ShopItems;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
        ShopItems = new List<ShopItemController>();
        ItemSoldList = new List<CardClass>();

        var sh = Screen.height;
        rect.offsetMin = new Vector2(rect.offsetMin.x, (sh / 2f) + 10);
        rect.offsetMax = new Vector2(rect.offsetMax.x, (sh / -2f) - 10);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public PlayerClass GetActivePlayer()
    {
        return ActivePlayer;
    }

    public void OpenShop(PlayerClass player)
    {
        ActivePlayer = player;

        var TownName = ActivePlayer.gameObject.GetComponent<PlayerMover>().GetWaypointName();

        if (!TownName.Equals(LastTownName))
        {
            LastTownName = TownName;

            ShopItems.Clear();
            foreach (Transform t in GridLayoutGroup_ShopItems.transform)
            {
                GameObject.Destroy(t.gameObject);
            }

            var cards = CardProvider.GetShopCards(TownName);
            foreach (var c in cards)
            {
                AddShopItem(c);
            }
        }

        var idx = 0;
        foreach (var c in player.Deck)
        {
            var obj = Instantiate(prefab_ShopItem, ShopInventoryController.Gridlayout_ShopInventory.transform);
            obj.GetComponent<ShopItemController>().GenerateShopItem(this, c, true);
            obj.name = "item_" + ++idx;
        }

        foreach (var item in ShopItems)
        {
            item.GetComponentInChildren<Button>(true).interactable = player.Gold >= item.Card.Price;
        }

        StartCoroutine(CoroutineOpenShop());
    }

    public void AddShopItem(CardClass card)
    {
        var obj = Instantiate(prefab_ShopItem, GridLayoutGroup_ShopItems.transform);
        obj.GetComponent<ShopItemController>().GenerateShopItem(this, card);
        ShopItems.Add(obj.GetComponent<ShopItemController>());
        obj.name = "item_" + ShopItems.Count;
    }

    public void RevalidateButtonsAndCosts()
    {
        foreach (Transform t in GridLayoutGroup_ShopItems.transform)
        {
            t.GetComponentInChildren<Button>(true).interactable =
                ActivePlayer.Gold - SumOfCosts >= t.GetComponent<ShopItemController>().Card.Price;
        }
        SumOfCostsText.text = "Total Cost:" + System.Environment.NewLine + SumOfCosts;
        ConfirmButton.interactable = SumOfCosts != 0;
    }
    
    public void AddToCart(Transform t)
    {
        SumOfCosts += t.GetComponent<ShopItemController>().Card.Price;
        CartController.AddToCart(this, t);

        RevalidateButtonsAndCosts();
    }

    public void RemoveFromCart(Transform t)
    {
        SumOfCosts -= t.GetComponent<ShopItemController>().Card.Price;
        CartController.RemoveFromCart(t);

        RevalidateButtonsAndCosts();
    }

    public void ItemSold(CardClass card)
    {
        ItemSoldList.Add(card);
        SumOfCosts -= Mathf.FloorToInt(card.Price * 0.3f);

        RevalidateButtonsAndCosts();
    }

    public void ItemUnSold(CardClass card)
    {
        ItemSoldList.Remove(card);
        SumOfCosts += Mathf.FloorToInt(card.Price * 0.3f);

        RevalidateButtonsAndCosts();
    }

    public void CloseShop()
    {
        if (ActivePlayer != null)
        {
            ActivePlayer = null;

            SumOfCosts = 0;
            ConfirmButton.interactable = false;
            CartController.EmptyCart();
            ShopInventoryController.Empty();

            StartCoroutine(CoroutineCloseShop());
        }
    }

    public void CommitShop()
    {
        if (SumOfCosts != 0)
        {
            ActivePlayer.Gold -= SumOfCosts;
            if (CartController.ItemCount > 0)
            {
                ActivePlayer.Deck.AddRange(CartController.GetCards());
            }
            
            foreach (var c in ItemSoldList)
            {
                ActivePlayer.Deck.Remove(c);
            }

            CloseShop();
        }
    }

    IEnumerator CoroutineOpenShop()
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentOffsetA = rect.offsetMin;
        Vector2 currentOffsetB = rect.offsetMax;
        Vector2 newOffsetA = new Vector2(currentOffsetA.x, 20);
        Vector2 newOffsetB = new Vector2(currentOffsetB.x, -20);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.offsetMin = Vector2.Lerp(currentOffsetA, newOffsetA, t);
            rect.offsetMax = Vector2.Lerp(currentOffsetB, newOffsetB, t);

            yield return null;
        }
    }

    IEnumerator CoroutineCloseShop()
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentOffsetA = rect.offsetMin;
        Vector2 currentOffsetB = rect.offsetMax;
        var sh = Screen.height;
        Vector2 newOffsetA = new Vector2(currentOffsetA.x, (sh / 2f) + 10);
        Vector2 newOffsetB = new Vector2(currentOffsetB.x, (sh / -2f) - 10);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.offsetMin = Vector2.Lerp(currentOffsetA, newOffsetA, t);
            rect.offsetMax = Vector2.Lerp(currentOffsetB, newOffsetB, t);

            yield return null;
        }
    }
}
