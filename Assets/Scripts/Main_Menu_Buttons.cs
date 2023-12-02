using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public Animator bookanimator;
    public GameObject waspMorhCanvas;
    public GameObject morphCanvas2;
    public GameObject settings;
    public bool isOpen = false;
    public GameObject wiki;
    public TMP_Text pointCounter;
    public AudioSource settingAudio;
    public AudioSource playAudio;
    public AudioSource morphAudio;
    public AudioSource wikiAudio;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        DisplayPoints();
        morphCanvas2.SetActive(true);
    }

    //Moves player to the platformer game world scene
    public void PlayGame()
    {
        playAudio.Play();
        gameManager.ChangeState(GameManager.GameState.LoadingEnvironment);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Update()
    {
        //DisplayPoints();
    }

    public void DisplayPoints()
    {
        pointCounter.text = gameManager.GetPlayerPoints().ToString();
    }

    public void Settings()
    {
        settingAudio.Play();
        settings.SetActive(!settings.activeSelf);
    }

    IEnumerator DelayFunc(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
    }

    public void Wiki()
    {
        wikiAudio.Play();
        wiki.SetActive(true);
        if (!isOpen)
        {
            bookanimator.SetTrigger("OpenBook");
            isOpen = true;
            StartCoroutine(DelayFunc(4));
        }
        else
        {
            bookanimator.SetTrigger("CloseBook");
            isOpen = false;
            StartCoroutine(DelayFunc(4));
        }
    }

    public void CanvasSlider()
    {
        if (!waspMorhCanvas.activeSelf)
        {
            waspMorhCanvas.SetActive(true);

        }
        else
        {
            waspMorhCanvas.SetActive(false);
        }
    }

    public void MorphCanvas2()
    {
        morphAudio.Play();
        waspMorhCanvas.SetActive(false);
        if (!morphCanvas2.activeSelf)
        {
            morphCanvas2.SetActive(true);
        }
        else
        {
            morphCanvas2.SetActive(false);
        }
    }

}