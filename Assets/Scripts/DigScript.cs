using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DigScript : MonoBehaviour
{
    GameManager gameManager;

    public GameObject maskPrefab;
    public EggSpawning EggSpawner;

    public GameObject preGameCanvas;
    public GameObject gameOverCanvas;
    public GameObject waspCanvas;
    public TMP_Text gameOverText;
    public TMP_Text preGameText;
    public TMP_Text timerText;

    public float timeLimit;
    private float currentTime;

    private int gameOver = 0;
    private int totalFoundEggs = 0;
    public int pointsEarned;

    public LineRenderer lineRenderer;
    
    // Variables for setting wasp sprite
    public SpriteRenderer waspSR;
    public Sprite longOvi, shortOvi;

    private void Start()
    {
        currentTime = timeLimit;
        UpdateTimerText();
        gameManager = GameManager.Instance;
        SetWaspSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (preGameCanvas.activeSelf)
        {
            HandlePreGame();
        }
        else
        {
            HandleTimer();
        }

        CheckEggs();

        if (gameOver == 1)
        {
            EndGame();
        }
    }

    private void HandlePreGame()
    {
        if (preGameCanvas.activeSelf)
        {
            preGameText.text = $"You have {timeLimit} seconds to find all the eggs \n\nTap to start";
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if the touch phase is "Began," indicating a new touch
            if (touch.phase == TouchPhase.Began)
            {
                preGameCanvas.SetActive(false);
            }
        }

    }

    void UpdateTimerText()
    {
        string formattedTime = currentTime.ToString("F2");
        timerText.text = formattedTime;
    }

    void HandleTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            currentTime = 0;
            gameOver += 1;
        }
    }

    void EndGame()
    {
        CalculatePoints();
        gameOverCanvas.SetActive(true);
        waspCanvas.SetActive(false);

        if (totalFoundEggs == EggSpawner.NumEggs)
        {
            gameOverText.text = $"You found all the eggs!\n\nPoints earned: {pointsEarned}";
        }
        else
        {
            gameOverText.text = $"You found {totalFoundEggs}/{EggSpawner.NumEggs} egg(s)\n\nPoints earned: {pointsEarned}";
        }
        StartCoroutine(WaitAndDoSomething(5));
    }

    //calculated points and saves to game manager
    private void CalculatePoints()
    {
        pointsEarned = totalFoundEggs * 50;
        gameManager.playerPoints += pointsEarned;
        Debug.Log("Added points");
    }

    void CheckEggs()
    {
        List<GameObject> eggs = EggSpawner.spawnedEggs;
        int n = eggs.Count;
        int foundEggs = 0;
        foreach (GameObject egg in eggs)
        {
            EggCollision eggCollision = egg.GetComponent<EggCollision>();
            if (eggCollision.found)
            { 
                foundEggs++;
            }
        }

        if (foundEggs == n)
        {
            gameOver += 1;
            totalFoundEggs = foundEggs;
        }
        else
        {
            totalFoundEggs = foundEggs;
        }
    }

    private IEnumerator WaitAndDoSomething(float x)
    {
        yield return new WaitForSeconds(x);

        // This code will run after waiting for 'delayInSeconds' seconds

        gameManager.ChangeState(GameManager.GameState.LoadingEnvironment);
    }

    // Set Sprite Based On Ovipositor
    void SetWaspSprite()
    {
        if (gameManager.waspData.hasLongOvi)
        {
            waspSR.sprite = longOvi;
        }
        else
        {
            waspSR.sprite = shortOvi;
        }
    }
}
