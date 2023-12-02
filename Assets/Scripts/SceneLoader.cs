using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField]
    private bool debugging = false;

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private Slider loadingSlider;

    [NonSerialized]
    public bool loading;

    public void LoadLevelBtn(string sceneToLoad)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(sceneToLoad));

        

    }

    IEnumerator LoadLevelAsync(string sceneToLoad)
    {
        int timer = 3;
        bool loadCompleted = false;
        Debug.LogError("LoadLevelAsync starting");
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        Debug.LogError("Started load operation");
        
        while (!loadCompleted)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return 1f;
            if (debugging)
                Debug.LogError("Loading...");

            timer--;

            if (timer <= 0)
                loadCompleted = true;
        }

        Debug.LogError("Loading finished, toggling LoadingScreen");

        loadingScreen.SetActive(false);

        GameManager.Instance.FindLoadedState();
    }
}
