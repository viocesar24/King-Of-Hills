using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    //public delegate void DialogFinishedHandler(object sender);
    //public event DialogFinishedHandler DialogFinished;

    private RectTransform rectTransform;
    private bool skipCurrent = false;
    private float initialY;

    public static bool IsShown = false;
    public static float TextSpeed = .01f;

    public Text TextSpeaker;
    public Text TextBox;
    private Queue<DialogText> DialogQueue;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        DialogQueue = new Queue<DialogText>();
        initialY = rectTransform.anchoredPosition.y;
    }

    public void EnqueueText(string speaker, string text)
    {
        DialogQueue.Enqueue(new DialogText() { speaker = speaker, text = text });
    }

    private void Show()
    {
        rectTransform.anchoredPosition += new Vector2(0, initialY * -1);
    }

    private void Hide()
    {
        rectTransform.anchoredPosition += new Vector2(0, initialY);
    }

    public void StartDialog(Action completed = null)
    {
        if (DialogQueue.Count > 0)
        {
            TextBox.text = "";
            Show();
            StartCoroutine(CoroutineTextAnimation(DialogQueue.Dequeue(), completed));
        }
        else
        {
            //DialogFinished(this);
            completed();
        }
    }

    public void SkipCurrentText()
    {
        skipCurrent = true;
    }

    
    IEnumerator CoroutineTextAnimation(DialogText dialog, Action completed)
    {
        TextBox.text = "";
        TextSpeaker.text = dialog.speaker;
        foreach (char c in dialog.text)
        {
            if (skipCurrent)
            {
                TextBox.text = dialog.text;
                skipCurrent = false;
                break;
            }
            TextBox.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }

        while (!skipCurrent)
        {
            yield return null;
        }

        skipCurrent = false;
        if (DialogQueue.Count > 0)
        {
            StartCoroutine(CoroutineTextAnimation(DialogQueue.Dequeue(), completed));
        }
        else
        {
            Hide();
            //DialogFinished(this);
            completed();
        }
    }

    private class DialogText
    {
        public string speaker;
        public string text;
    }
}
