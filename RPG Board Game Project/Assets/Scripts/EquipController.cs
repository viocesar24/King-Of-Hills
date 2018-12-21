using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipController : MonoBehaviour {

    public GameObject GridLayout_InventoryEquipment;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EquipCard(Transform item)
    {
        item.SetParent(GridLayout_InventoryEquipment.transform);
        item.GetComponent<CardInventoryController>().ActionButton.GetComponentInChildren<Text>().text = "Unequip";
    }


    public void EmptyEquipment()
    {
        foreach (Transform item in GridLayout_InventoryEquipment.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}
