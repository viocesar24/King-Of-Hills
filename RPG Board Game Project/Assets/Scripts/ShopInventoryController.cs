using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryController : MonoBehaviour {

    public GameObject Gridlayout_ShopInventory;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Empty()
    {
        foreach (Transform t in Gridlayout_ShopInventory.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }
}
