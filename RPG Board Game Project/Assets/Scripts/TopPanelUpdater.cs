using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelUpdater : MonoBehaviour {

    public static TopPanelUpdater instance;
    private bool GameStopped;
    private Vector3 HiddenPosition;
    private Vector3 DefaultPosition;
    private RectTransform rectTransform;
    private TimeSpan duration;

    public Text TextDuration;
    public Text TextTurn;


	// Use this for initialization
	void Start ()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        HiddenPosition = rectTransform.anchoredPosition; // hidden by default;
        DefaultPosition = new Vector2(HiddenPosition.x, 0);

        duration = new TimeSpan(0, 0, 0);
    }
	
	
    public void StartUpdater()
    {
        GameStopped = false;
        StartCoroutine(GameDurationCounter());
    }

    public void NextTurn()
    {
        TextTurn.text = string.Format("{0:00}", GameController.Turn);
    }

    public void HidePanel()
    {
        StartCoroutine(CoroutineHidePanel());
    }

    public void ShowPanel()
    {
        StartCoroutine(CoroutineShowPanel());
    }

    IEnumerator GameDurationCounter()
    {
        while (!GameStopped)
        {
            TextDuration.text = string.Format("{0:00}:{1:00}", duration.Minutes, duration.Seconds);
            yield return new WaitForSeconds(1);
            duration = duration.Add(TimeSpan.FromSeconds(1));
            
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
