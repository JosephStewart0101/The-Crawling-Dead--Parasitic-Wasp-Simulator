using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CaterpillarGameManager : Singleton<CaterpillarGameManager>
{
    public enum MiniState
    {
        menu,
        starting,
        playing,
        paused,
        ending
    }

    [SerializeField]
    private bool debugging = false;

    public MiniState miniState = MiniState.menu;

    public event EventHandler OnCatMiniStateChanged;   // On minigame state changed.

    public event EventHandler OnPlayerDeath;           // The player has died,
                                                       // all hope is lost. 
                                                       // God save the queen.

    public event EventHandler OnFrenzyStarted;         // Frenzy has begun, run.
    public event EventHandler OnFrenzyEnded;           // Frenzy has ended, welcome back.

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private bool playerIsAlive = true;

    public GameObject menu;         // Menu
    public GameObject mainMenu;     // Main Menu
    public GameObject endMenu;      // End Menu

    public Button startButton;      // Starts the minigame
    public Button pauseButton;      // Pauses the minigame
    public Button resumeButton;     // Resumes from pause-menu
    public Button exitButton;       // Exits from score menu or 

    public int nTotalRounds = 1;    // Number of rounds the user should have 
                                    // to complete before the minigame ends

    public GameObject Player;
    public GameObject GhostPrefab;

    public float playerSpeed;
    public bool isPlayerManic;      // For when the player iniates a ghost-flee

    private int countdownTime = 3;

    private WaitForSeconds StartWait;
    private WaitForSeconds EndWait;

    public float ghostSpeed = 2f;       // Base speed for ghosts
    [SerializeField]
    private float ghostChaseSpeed = 5f;

    //[SerializeField]
    private float ghostAggression = 5f;   // Aggression range
    //[SerializeField]
    private int ghostCount = 3;
    //[SerializeField]
    private int eggsPossible; 

    public Transform ghostSpawn;

    public GhostBehavior[] ghostList;

    private float compatibility;
    public float difficulty;

    public bool gatesAreClosed = false;

    [SerializeField]
    private Image joystickHandle;
    [SerializeField]
    private Image joystickOutline;
    [SerializeField]
    private GameObject scoreObject;

    public ScoreInt caterpillarScore;
    public GameObject scoreTextUpdater;

    private int pointsEarned;

    private bool playerWon;

    public Transform playerTransform;

    public override void Start()
    {
        playerTransform.localScale = new Vector3(2.5f, 2.5f, 0.833333254f);

        ReadDifficulty(); // Difficulty function
        Debug.LogError("Read the difficulty");

        joystickHandle.enabled = false;
        joystickOutline.enabled = false;
        scoreObject.SetActive(false);

        // Initialize some menu values
        endMenu.SetActive(false);

        // Play game audio
        GetComponent<AudioSource>().Play();

        playerMovement = Player.GetComponent<PlayerMovement>();

        // Set player speed
        playerMovement.speed = playerSpeed;

        // Add listener for buttons button
        startButton.onClick.AddListener(StartMinigame);
        //pauseButton.onClick.AddListener(PauseMinigame);
        //resumeButton.onClick.AddListener(ResumeMinigame);

        exitButton.onClick.AddListener(ExitMinigame);
        exitButton.enabled = false;

        startButton.enabled = true;
        // Hide buttons that aren't necessary right now
        //pauseButton.enabled = false;
        //resumeButton.enabled = false;
        //exitButton.enabled = true;

        compatibility = GameManager.Instance.currentHostCompatibility;
        
        
    }



    public void ExitMinigame()
    {
        GameManager.Instance.LoadEnvironment(GameManager.GameState.CaterpillarMG);
    }

    /// <summary>
    /// Actual start function for the minigame
    /// </summary>
    public void StartMinigame()
    {
        playerTransform.localScale = new Vector3(2.5f, 2.5f, 0.833333254f);
        joystickHandle.enabled = true;
        joystickOutline.enabled = true;
        scoreObject.SetActive(true);

        ghostList = new GhostBehavior[ghostCount];  // Initialize list size

        playerIsAlive = true;       // This is for safety

        if (debugging)
            Debug.LogError("StartMinigame");

        // Disable menu & main menu - shouldn't need to disable button(s)
        mainMenu.SetActive(false);
        menu.SetActive(false);

        StartCoroutine(GameLoop());
        
    }

    /// <summary>
    /// To pause and resume the minigame. These are TO-DO.
    /// </summary>
    public void PauseMinigame()
    {

    }
    public void ResumeMinigame()
    {

    }

    /// <summary>
    /// The player has died, let everybody know
    /// </summary>
    public void RaisePlayerDeathEvent()
    {
        if (debugging)
            Debug.LogError("GM::RaisePlayerDeathEvent");
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        playerIsAlive = false;
        ChangeMiniState(MiniState.ending);
    }

    public void RaisePlayerFrenzyEvent()
    {
        if (debugging)
            Debug.LogError("GM::RaisePlayerFrenzyEvent");
        OnFrenzyStarted?.Invoke(this, EventArgs.Empty);
        isPlayerManic = true;
        StartCoroutine(ManicTimer());
    }
    private DateTime doubleManicCheck;
    private IEnumerator ManicTimer()
    {
        // Save this to verify another manic episode hasn't started during our process
        DateTime currentSystemTime = System.DateTime.Now;
        doubleManicCheck = currentSystemTime; 
        
        for (int i = 10; i >= 0; i--)
        {
            Debug.LogError("GM::ManicTimer: isPlayerManic = " + isPlayerManic);
            yield return new WaitForSeconds(1f);
        }

        // If this returns false, we shouldn't 
        if (currentSystemTime == doubleManicCheck)
        {
            isPlayerManic = false;
            OnFrenzyEnded?.Invoke(this, EventArgs.Empty);
        }
    }


    /// <summary>
    /// Take difficulty information from the game manager
    /// </summary>
    private void ReadDifficulty()
    {
        if (GameManager.Instance != null)
            difficulty = (1f - compatibility) * 20;
        Debug.Log("Caterpillar minigame difficulty set at " + difficulty + " out of 10");


        // Determine what the difficulty means
        if (difficulty >= 9 || difficulty < 0)
        {
            ghostCount += 3;
            ghostSpeed = 4.5f;
            playerSpeed = 6.3f;

            gatesAreClosed = false; // Disable the gates (warps)
        }
        else if (difficulty >= 8)
        {
            ghostCount += 2;
            ghostSpeed = 4.2f;
            playerSpeed = 6.2f;
        }
        else if (difficulty >= 6)
        {
            ghostCount += 2;
            ghostSpeed = 4f;
            playerSpeed = 6.1f;
        }
        else if (difficulty >= 4)
        {
            ghostCount += 1;
            ghostSpeed = 3.7f;
        }
        else if (difficulty >= 2)
        {
            ghostCount += 1;
            ghostSpeed = 3.4f;

        }
        else 
        {
            Debug.Log("Difficulty very low, don't change anything.");
        }
    }

    
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());

        yield return StartCoroutine(GamePlaying());

        yield return StartCoroutine(GameEnding());
    }
        private IEnumerator GameStarting()
    {
        if (debugging)
            Debug.LogError("GameStarting");

        yield return new WaitForSeconds(1f);

        Instantiate(GhostPrefab, ghostSpawn);

        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(1f);

        yield return null;

    }

    public void SpawnNewGhost()
    {
        Instantiate(GhostPrefab, ghostSpawn);
    }
    
    private IEnumerator GamePlaying()
    {
        miniState = MiniState.playing;
        Debug.LogError("GamePlaying");

        int tempGhostCount = ghostCount - 1;

        while (playerIsAlive)
        {
            for (int timer = 0; timer < 8; timer++)
            {
                if (!playerIsAlive)
                    timer = 8;
                else
                    yield return new WaitForSeconds(1f);
            }
            if (tempGhostCount >= 0 && playerIsAlive)
            {
                Instantiate(GhostPrefab, ghostSpawn);
                tempGhostCount--;       // We just spawned a ghost, mark it in tempGhostCount
            }

            yield return null;
        }
        Debug.LogError("Should be ending GamePlaying");
        // One of the conditions have failed... tell everybody to end the game
        if (miniState != MiniState.ending)
            ChangeMiniState(MiniState.ending);
    }

    
    private IEnumerator GameEnding()
    {
        Debug.LogError("Should be in GameEnding");

        joystickHandle.enabled = false;
        joystickOutline.enabled = false;
        scoreObject.transform.position = Vector2.zero;

        playerMovement.speed = 0;
        menu.SetActive(true);
        endMenu.SetActive(true);
        exitButton.enabled = true;
        FinalPoints();
        yield return null;
    }

    private void FinalPoints()
    {
        if (playerWon)
            pointsEarned = caterpillarScore.score/2;
        else
            pointsEarned = caterpillarScore.score/6;

        // Update the primary GameManager
        GameManager.Instance.playerPoints += pointsEarned;
    }

    public void ChangeMiniState(MiniState newState)
    {
        miniState = newState;
        OnCatMiniStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetPlayerWinLose(bool win)
    {
        if (win)
            playerWon = true;
        else
            playerWon = false;
    }

    /// <summary>
    /// Reward the player with 6 points for killing a ghost
    /// </summary>
    public void GiveGhostKillPoints()
    {
        caterpillarScore.score += 6;
        ScoreTextUpdater value = scoreTextUpdater.GetComponent<ScoreTextUpdater>();
        value.OnScoreUpdate?.Invoke();
    }
}
