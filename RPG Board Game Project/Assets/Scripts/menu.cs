using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour {

    public GameObject LoadingScreen;
    public Slider slider;
    public Text progresstext;

    public void PlayGame(int sceneIndex)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progresstext.text = progress * 100f + "%";

            yield return null;

        }
    }
}
