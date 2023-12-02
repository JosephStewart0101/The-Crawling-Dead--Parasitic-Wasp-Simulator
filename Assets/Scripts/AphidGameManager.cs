using System.Reflection;
using System.Threading;
using TMPro;
using UnityEngine;

public class AphidGameManager : MonoBehaviour
{
    public float timeLimit = 20f;
    public float numAntSpawns;
    public GameDifficulty difficulty;
    public float spawnDelay = 0.3f;
    private float nextTimeToSpawn = 0f;
    public GameObject ant;
    public GameObject IntroCanvas;
    public GameObject GameOverCanvas;
    public TMP_Text introText;
    public TMP_Text GameOverText;
    public Transform[] spawnPoints; // must manually create array size on game object
    public Transform[] chosenSpawns; // must manually create array size on game object
    public static bool winningCondition;
    public static bool gameOver;
    public static bool addedPoints;


    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    void Awake()
    {
        addedPoints = false;
        gameOver = false;
        IntroCanvas.SetActive(true); // makes sure intro canvas is active
        GameOverCanvas.SetActive(false); // makes sure the game over screen isn't active
        FindDifficulty();
        ChooseSpawnPoints(chosenSpawns);  
    }

    private void Update()
    {
        // control spawning of ants. Only spawns ants when game over screen isn't active
        if (nextTimeToSpawn <= Time.time && gameOver == false)
        {
            SpawnAnt();
            nextTimeToSpawn = Time.time + spawnDelay;
        }

        // displays intro message and waits for a touch to deactivate
        if (IntroCanvas.activeSelf)
        {
            Intro();
        }
        

        if (gameOver == true)
        {
            GameOver();
        }

    }

    public void FindDifficulty()
    {
        float x = GameManager.Instance.currentHostCompatibility;
        Debug.Log("Difficulty rating calculated: " + x);

        if (x > 0.5 && x < 0.65)
        {
            SetDifficulty(GameDifficulty.Hard);
        }
        else if (x > 0.65 && x < 0.8)
        {
            SetDifficulty(GameDifficulty.Medium);
        }
        else
        {
            SetDifficulty(GameDifficulty.Easy);
        }
    }

    void ChooseSpawnPoints(Transform[] chosenSpawns)
    {
        int j = 0;
        for (int i = 0; i < spawnPoints.Length; i += 2)
        {
            if (j >= numAntSpawns)
                break;
            int firstOfSpawnPair = i;
            int secondOfSpawnPair = i + 1;
            int chosenSpawn = Random.Range(firstOfSpawnPair, secondOfSpawnPair + 1); // Random.Range excludes the max number. Added 1 as workaround
            chosenSpawns[j] = spawnPoints[chosenSpawn];
            Debug.Log("Spawn point chosen: " + chosenSpawn);
            j++;
        }
    }

    public void SetDifficulty(GameDifficulty newDifficulty)
    {
        difficulty = newDifficulty;

        // Set the time limit based on the selected difficulty
        switch (newDifficulty)
        {
            case GameDifficulty.Easy:
                numAntSpawns = 3;
                Debug.Log("Difficulty set to easy. numAntSpawns = " + numAntSpawns);
                break;
            case GameDifficulty.Medium:
                numAntSpawns = 4;
                Debug.Log("Difficulty set to medium. numAntSpawns = " + numAntSpawns);
                break;
            case GameDifficulty.Hard:
                numAntSpawns = 5;
                Debug.Log("Difficulty set to hard. numAntSpawns = " + numAntSpawns);
                break;
            default:
                Debug.LogError("Unknown difficulty level");
                break;
        }
    }

    void SpawnAnt()
    {
        // picks a random spawn point from the chosenSpawns array
        int randomIndex = Random.Range(0, chosenSpawns.Length);
        Transform spawnPoint = chosenSpawns[randomIndex];
        Instantiate(ant, spawnPoint.position, spawnPoint.rotation);
    }

    void Intro()
    {
        if (IntroCanvas.activeSelf)
        {
            introText.text = $"These ants are protecting the aphid to collect its honeydew.\n" +
                             $"Avoid the ants to parasitize the aphid!\n\nTap to anywhere to continue...";
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if the touch phase is "Began," indicating a new touch
            if (touch.phase == TouchPhase.Ended)
            {
                IntroCanvas.SetActive(false);
            }
        }
    }

    public void GameOver()
    {
        //Debug.Log("Called the game over script in AphidGameManager.cs");
        GameOverCanvas.SetActive(true);

        if (winningCondition == true)
        {
            GameOverText.text = "You've successfully parasitized the aphid! You've earned 50 points.";
            GameManager.Instance.currentHost.Parasitized?.Invoke(); // unlocks the aphid's wiki page

            if (addedPoints == false)
            {
                GameManager.Instance.playerPoints += 50; // adds 50 points to player's total
                Debug.Log("Added points for wining aphid minigame");
                addedPoints = true;
            }
        }

        if (winningCondition == false)
        {
            GameOverText.text = "The ants prevented you from parasitizing the aphid!";
            
        }
        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if the touch phase is "Began," indicating a new touch
            if (touch.phase == TouchPhase.Ended)
            {
                Thread.Sleep(300);
                if (winningCondition == true)
                {
                    GameManager.Instance.playerPoints += 50; // adds 50 points to player's total
                }
                GameManager.Instance.ChangeState(GameManager.GameState.LoadingEnvironment); // places the player back in the overworld
            }
        }
        */
    }
}
