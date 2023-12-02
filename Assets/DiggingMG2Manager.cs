using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiggingMG2Manager : MonoBehaviour
{
    public GameObject maskPrefab;
    public EggSpawning EggSpawner;
    public GameObject preGameCanvas;
    public GameObject gameOverCanvas;
    public GameObject timerCanvas;
    public GameObject waspCanvas;
    public GameObject frontSprite;
    public GameObject grass;
    public GameObject underground;
    public TMP_Text gameOverText;
    public TMP_Text preGameText;
    public TMP_Text timerText;
    public float timeLimit;
    private float currentTime;
    private int gameOver = 0;
    private int totalFoundEggs = 0;
    public int pointsEarned;
    public LineRenderer lineRenderer;
    public GameDifficulty difficulty;

    // Variables for setting wasp sprite
    public SpriteRenderer waspSR;
    public Sprite longOvi, shortOvi;

    public Transform needleTransform;
    public float rotationSpeed = 50.0f;
    private float currentRotation = 0.0f;
    private float rotationDirection = 1.0f;
    public float lineCooldown;
    public bool canCreateLine = true;
    private bool isTouchingScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;
        lineRenderer.positionCount = 0; // Initialize with no points
        UpdateTimerText();
        SetWaspSprite();
        FindDifficulty();
        
        if (GameManager.Instance.playerPos.y < -30)
        {
            SwitchBackground();
        }

    }

    // Update is called once per frame
    void Update()
    {
        GameLoop();

        MoveNeedle();

        //Only check for taps after pre game canvas is done
        if (!preGameCanvas.activeSelf && !gameOverCanvas.activeSelf)
        {
            CheckForTaps();
        }
    }

    private void GameLoop()
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
            preGameText.text = $"Use your ovipositor to dig through the dirt in search of beetle eggs.\n" +
                               $"You have {timeLimit} seconds to find all the eggs \n\nTap to anywhere to continue...";
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if the touch phase is "Began," indicating a new touch
            if (touch.phase == TouchPhase.Ended)
            {
                preGameCanvas.SetActive(false);
                timerCanvas.SetActive(true);
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
        timerCanvas.SetActive(false);
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
        GameManager.Instance.currentHost.Parasitized?.Invoke();
        //StartCoroutine(WaitAndDoSomething(5));
    }

    //calculated points and saves to game manager
    private void CalculatePoints()
    {
        pointsEarned = totalFoundEggs * 25;
        GameManager.Instance.playerPoints += pointsEarned;
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

    // Set Sprite Based On Ovipositor
    void SetWaspSprite()
    {
        if (GameManager.Instance.waspData.hasLongOvi)
        {
            waspSR.sprite = longOvi;
        }
        else
        {
            waspSR.sprite = shortOvi;
        }
    }

    private void CheckForTaps()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                if (canCreateLine)
                {
                    isTouchingScreen = true;
                    //CreateLine();
                    CreateMask();
                    StartCoroutine(LineCooldown()); // Start the cooldown timer.
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouchingScreen = false;
            }
        }
    }

    private IEnumerator LineCooldown()
    {
        yield return new WaitForSeconds(lineCooldown);
        canCreateLine = true; // Allow creating lines again after cooldown.
    }

    private void MoveNeedle()
    {
        if (needleTransform != null)
        {
            currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;
            needleTransform.localRotation = Quaternion.Euler(0, 0, currentRotation);

            if (currentRotation >= 87.0f || currentRotation <= -87.0f)
            {
                rotationDirection *= -1.0f;
            }
        }
    }

    void CreateMask()
    {
        // Get the current angle of the needle
        float currentAngle = needleTransform.rotation.eulerAngles.z;
        currentAngle += 180;

        // Calculate the direction vector based on the needle angle
        Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.up;

        // Set the positions of the line's start and end points
        Vector3 startPoint = needleTransform.position;
        Vector3 endPoint = needleTransform.position + direction * 15f;
        Vector3 midpoint = CalculateMidpoint(startPoint, endPoint);

        // Instantiate the object to be masked
        GameObject objectToMask = Instantiate(frontSprite, Vector3.zero, Quaternion.identity);

        // Create a new GameObject for the mask
        GameObject maskObject = new GameObject("SpriteMask");
        maskObject.transform.position = midpoint;
        maskObject.transform.localScale = new Vector3(20f, 0.5f, 1f);
        float trueAngle = ((currentAngle - 180) + 360) % 360;
        if (trueAngle > 0)
        {
            trueAngle += 90;
        }
        else
        {
            trueAngle = 90 - (-trueAngle);
        }
        maskObject.transform.rotation = Quaternion.Euler(0, 0, trueAngle);

        // Add the SpriteMask component to the mask object
        SpriteMask spriteMask = maskObject.AddComponent<SpriteMask>();
        spriteMask.sprite = objectToMask.GetComponent<SpriteRenderer>().sprite;// Assign the object to be masked to the sprite of the SpriteMask component
        spriteMask.isCustomRangeActive = false; // Set the mask interaction option

        // Add a BoxCollider2D to the mask object
        BoxCollider2D boxCollider = maskObject.AddComponent<BoxCollider2D>();

        // Destroy the original object to be masked (optional)
        Destroy(objectToMask);

        canCreateLine = false;
    }

    Vector3 CalculateMidpoint(Vector3 point1, Vector3 point2)
    {
        float xMidpoint = (point1.x + point2.x) / 2f;
        float yMidpoint = (point1.y + point2.y) / 2f;
        float zMidpoint = (point1.z + point2.z) / 2f;

        return new Vector3(xMidpoint, yMidpoint, zMidpoint);
    }

    public void SetDifficulty(GameDifficulty newDifficulty)
    {
        difficulty = newDifficulty;

        // Set the time limit based on the selected difficulty
        switch (newDifficulty)
        {
            case GameDifficulty.Easy:
                timeLimit = 30f;
                break;
            case GameDifficulty.Medium:
                timeLimit = 20f;
                break;
            case GameDifficulty.Hard:
                timeLimit = 10f;
                break;
            default:
                Debug.LogError("Unknown difficulty level");
                break;
        }
    }

    public void FindDifficulty()
    {
        float x = GameManager.Instance.currentHostCompatibility;

        if (x > 0.5 && x <= 0.7)
        {
            SetDifficulty(GameDifficulty.Hard);
        }
        else if (x > 0.7 && x < 0.8)
        {
            SetDifficulty(GameDifficulty.Medium);
        }
        else
        {
            SetDifficulty(GameDifficulty.Easy);
        }
    }

    private void SwitchBackground()
    {
        grass.SetActive(false);
        underground.SetActive(true);
    }

    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
