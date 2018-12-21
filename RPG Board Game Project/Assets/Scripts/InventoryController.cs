using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public RectTransform InventoryRect;
    
    public Transform prefab_InventoryItem;
    public GameObject Gridlayout_InventoryItems;
    public EquipController EquipController;
    public EquipPreviewController EquipPreviewController;

    private PlayerClass ActivePlayer;

    // Use this for initialization
    void Start () {
        //InventoryRect = GetComponent<RectTransform>();

        var sh = Screen.height;
        InventoryRect.offsetMin = new Vector2(InventoryRect.offsetMin.x, (sh / 2f) + 10);
        InventoryRect.offsetMax = new Vector2(InventoryRect.offsetMax.x, (sh / -2f) - 10);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenInventory()
    {
        GameController.instance.HideBottomPanel();
        foreach (Transform item in Gridlayout_InventoryItems.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        EquipController.EmptyEquipment();

        var player = GameController.instance.GetCurrentPlayer();
        GenerateInventoryItems(player);
        StartCoroutine(CoroutineOpenInventory());
    }

    public void CloseInventory()
    {
        EquipPreviewController.RegenerateItems(ActivePlayer.Equipped);
        GameController.instance.BottomPanelUpdater.UpdatePanel();
        StartCoroutine(CoroutineCloseInventory());
        if (!TownMenuController.IsShowing)
        {
            GameController.instance.ShowBottomPanel();
        }
    }

    public void GenerateInventoryItems(PlayerClass player)
    {
        ActivePlayer = player;
        foreach (var c in player.Deck)
        {
            var e = Instantiate(prefab_InventoryItem, Gridlayout_InventoryItems.transform);
            e.GetComponent<CardInventoryController>().GenerateInventoryItem(this, c);
            e.GetComponent<CardInventoryController>().ActionButton.GetComponentInChildren<Text>().text = "Equip";
        }

        foreach (var c in player.Equipped)
        {
            var e = Instantiate(prefab_InventoryItem, transform);
            e.GetComponent<CardInventoryController>().GenerateInventoryItem(this, c);
            EquipController.EquipCard(e);
        }
    }

    public void EquipCard(Transform item)
    {
        ActivePlayer.Equipped.Add(item.GetComponent<CardInventoryController>().Card);
        ActivePlayer.Deck.Remove(item.GetComponent<CardInventoryController>().Card);
        EquipController.EquipCard(item);
    }

    public void UnequipCard(Transform item)
    {
        ActivePlayer.Equipped.Remove(item.GetComponent<CardInventoryController>().Card);
        ActivePlayer.Deck.Add(item.GetComponent<CardInventoryController>().Card);
        item.SetParent(Gridlayout_InventoryItems.transform);
        item.GetComponent<CardInventoryController>().ActionButton.GetComponentInChildren<Text>().text = "Equip";
    }

    IEnumerator CoroutineOpenInventory()
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentOffsetA = InventoryRect.offsetMin;
        Vector2 currentOffsetB = InventoryRect.offsetMax;
        Vector2 newOffsetA = new Vector2(currentOffsetA.x, 0);
        Vector2 newOffsetB = new Vector2(currentOffsetB.x, 0);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            InventoryRect.offsetMin = Vector2.Lerp(currentOffsetA, newOffsetA, t);
            InventoryRect.offsetMax = Vector2.Lerp(currentOffsetB, newOffsetB, t);

            yield return null;
        }
    }

    IEnumerator CoroutineCloseInventory()
    {
        var sh = Screen.height;

        float elapsed = 0;
        float t = 0;
        Vector2 currentOffsetA = InventoryRect.offsetMin;
        Vector2 currentOffsetB = InventoryRect.offsetMax;
        Vector2 newOffsetA = new Vector2(currentOffsetA.x, (sh / 2f) + 10);
        Vector2 newOffsetB = new Vector2(currentOffsetB.x, (sh / -2f) - 10);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            InventoryRect.offsetMin = Vector2.Lerp(currentOffsetA, newOffsetA, t);
            InventoryRect.offsetMax = Vector2.Lerp(currentOffsetB, newOffsetB, t);

            yield return null;
        }
    }
}
