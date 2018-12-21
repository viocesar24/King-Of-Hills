using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMenuController : MonoBehaviour {
    public static bool IsShowing = false;
    public ShopController ShopController;

    public GameObject ExitObject;
    public GameObject SleepObject;

    private PlayerClass player;
    private RectTransform rect;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenMenu(PlayerClass p)
    {
        player = p;
        var moveRemaining = p.gameObject.GetComponent<PlayerMover>().GetMoveRemaining();
        if (moveRemaining == 0)
        {
            ExitObject.SetActive(false);
            SleepObject.SetActive(true);
        }
        else
        {
            ExitObject.SetActive(true);
            SleepObject.SetActive(false);
        }

        StartCoroutine(CoroutineOpenMenu());
        IsShowing = true;
    }
    

    public void ShopClick()
    {
        ShopController.OpenShop(player);
    }

    public void ExitClick()
    {
        ShopController.CloseShop();
        StartCoroutine(CoroutineCloseMenu());
    }

    public void SleepCLick()
    {
        ShopController.CloseShop();
        player.Lives = 5;
        StartCoroutine(CoroutineCloseMenu());
    }

    IEnumerator CoroutineOpenMenu()
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentDelta = rect.sizeDelta;
        Vector2 newDelta = new Vector2(currentDelta.x, 150);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.sizeDelta = Vector2.Lerp(currentDelta, newDelta, t);

            yield return null;
        }
    }

    IEnumerator CoroutineCloseMenu()
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentDelta = rect.sizeDelta;
        Vector2 newDelta = new Vector2(currentDelta.x, 0);

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.sizeDelta = Vector2.Lerp(currentDelta, newDelta, t);

            yield return null;
        }

        StartCoroutine(CoroutineHideTownDialog());
    }

    IEnumerator CoroutineHideTownDialog()
    {
        float elapsed = 0;
        float t = 0;
        Color curColor = GameController.instance.TownBackground.color;
        var currentAlpha = curColor.a;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            curColor.a = Mathf.Lerp(currentAlpha, 0f, t);
            GameController.instance.TownBackground.color = curColor;
            yield return null;
        }
        yield return new WaitForSeconds(.5f);

        GameController.instance.ShowBottomPanel();
        player.gameObject.GetComponent<PlayerMover>().PauseMove(false);

        IsShowing = false;
    }
}
