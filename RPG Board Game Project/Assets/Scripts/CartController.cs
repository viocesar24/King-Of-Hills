using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartController : MonoBehaviour {

    public GameObject GridLayoutGroup_Cart;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToCart(ShopController controller, Transform t)
    {
        var card = t.GetComponent<ShopItemController>().Card;
        var tr = Instantiate(t, GridLayoutGroup_Cart.transform);
        tr.GetComponent<ShopItemController>().GenerateShopItem(controller, card);
        tr.GetComponent<ShopItemController>().BuyButtonObject.GetComponentInChildren<Text>().text = "Remove";
    }

    public void EmptyCart()
    {
        foreach (Transform t in GridLayoutGroup_Cart.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    public void RemoveFromCart(Transform t)
    {
        GameObject.Destroy(t.gameObject);
    }

    public int ItemCount
    {
        get
        {
            int c = 0;
            foreach (Transform t in GridLayoutGroup_Cart.transform)
            {
                c++;
            }

            return c;
        }
    }
    
    public List<CardClass> GetCards()
    {
        var list = new List<CardClass>();
        foreach (Transform t in GridLayoutGroup_Cart.transform)
        {
            var sic = t.GetComponent<ShopItemController>();
            list.Add(sic.Card);
        }
        return list;
    }
}
