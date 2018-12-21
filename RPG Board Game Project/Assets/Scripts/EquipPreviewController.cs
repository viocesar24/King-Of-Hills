using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipPreviewController : MonoBehaviour {


    public Transform prefab_EquipCard;
    public GameObject Gridlayout_EquipPreview;
    public Text NoCardText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegenerateItems(List<CardClass> cards)
    {
        foreach (Transform t in Gridlayout_EquipPreview.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        if (cards.Count == 0)
        {
            NoCardText.gameObject.SetActive(true);
            return;
        }

        NoCardText.gameObject.SetActive(false);
        foreach (var c in cards)
        {
            var a = Instantiate(prefab_EquipCard, Gridlayout_EquipPreview.transform);
            a.GetComponent<CardEquipController>().GenerateCardEquipPreview(this, c);
        }
    }
}
