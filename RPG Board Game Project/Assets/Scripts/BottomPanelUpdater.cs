using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelUpdater : MonoBehaviour {

    public static BottomPanelUpdater instance;
    private PlayerClass player = null;
    private Vector3 HiddenPosition;
    private Vector3 DefaultPosition;
    private RectTransform rectTransform;

    Image player_img;
    Text player_name;
    GameObject live_container;
    Text att_val;
    Text gold_val;

    public EquipPreviewController EquipPreviewController;

    // Use this for initialization
    void Start () {
        var rm = transform.GetChild(0);
        player_img = rm.GetChild(0).gameObject.GetComponent<Image>();
        player_name = rm.GetChild(1).gameObject.GetComponent<Text>();
        live_container = rm.GetChild(2).gameObject;
        att_val = rm.GetChild(3).gameObject.GetComponent<Text>();
        gold_val = rm.GetChild(4).gameObject.GetComponent<Text>();

        instance = this;
        rectTransform = GetComponent<RectTransform>();
        HiddenPosition = rectTransform.anchoredPosition; // hidden by default;
        DefaultPosition = new Vector2(HiddenPosition.x, 0);
    }

    public void HidePanel()
    {
        StartCoroutine(CoroutineHidePanel());
    }

    public void ShowPanel()
    {
        StartCoroutine(CoroutineShowPanel());
    }

    public void SetPlayer(PlayerClass player)
    {
        this.player = player;
        EquipPreviewController.RegenerateItems(player.Equipped);
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        player_img.sprite = player.Image;
        player_name.text = player.Name;
        SetLives(player.Lives);
        att_val.text = player.ATK.ToString();
        var goldBonus = player.Equipped.Where(a => a.Type == CardClass.CardType.GOLD).Sum(a => a.Value);
        gold_val.text = player.Gold.ToString() + (goldBonus > 0 ? " (+" + goldBonus + "%)": "");
    }

    private void SetLives(int count)
    {
        foreach (Transform item in live_container.transform)
        {
            item.gameObject.SetActive(false);
        }
        if (count > 5)
        {
            count = 5;
        }
        for (int i = 0; i < count; i++)
        {
            live_container.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    IEnumerator CoroutineHidePanel()
    {
        float elapsed = 0;
        float t = 0;
        Vector3 currentPosition = rectTransform.anchoredPosition;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / 0.3f));
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, HiddenPosition, t);

            yield return null;
        }
    }

    IEnumerator CoroutineShowPanel()
    {
        float elapsed = 0;
        float t = 0;
        Vector3 currentPosition = rectTransform.anchoredPosition;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / 0.3f));
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, DefaultPosition, t);

            yield return null;
        }
    }
}
