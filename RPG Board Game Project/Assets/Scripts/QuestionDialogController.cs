using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogController : MonoBehaviour {

    public Text TextBox;
    public Text OptionA;
    public Text OptionB;

    private RectTransform rect;
    private int answer = 0; // 0: not answered | 1: option A | 2: option B
    private bool AllowAnswer = false;

    // Use this for initialization
    void Start () {
        rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowDialog(string text, string optionA, string optionB, Action<bool, string> completed)
    {
        answer = 0;
        TextBox.text = text;
        OptionA.text = optionA;
        OptionB.text = optionB;

        StartCoroutine(CoroutineShowDialog(completed));
    }

    public void OptionA_OnClick()
    {
        if (AllowAnswer)
        {
            answer = 1;
        }
    }

    public void OptionB_OnClick()
    {
        if (AllowAnswer)
        {
            answer = 2;
        }
    }

    IEnumerator CoroutineShowDialog(Action<bool, string> completed)
    {
        float elapsed = 0;
        float t = 0;
        Vector2 currentDelta = rect.sizeDelta;
        Vector2 newDelta = new Vector2(currentDelta.x, 155);
        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.sizeDelta = Vector2.Lerp(currentDelta, newDelta, t);

            yield return null;
        }

        AllowAnswer = true;
        while (answer < 1)
        {
            yield return null;
        }
        AllowAnswer = false;

        t = 0;
        elapsed = 0;
        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            rect.sizeDelta = Vector2.Lerp(newDelta, currentDelta, t);

            yield return null;
        }

        completed(answer == 1, answer == 1 ? OptionA.text : OptionB.text);
    }
}
