using UnityEngine;
using TMPro;

enum CTDCurrentState
{
    Intro, Starting, Playing, Ending
}

public enum CTDDifficulty
{
    Easy, Normal, Hard
}


// Logic for Connect The Dots minigame
public class ConnectTheDotsMGManager : MonoBehaviour
{
    // Variables that change based on difficulty
    struct CTDDiffVars
    {
        public int maxTries, numDots;
    }

    public DotComponent dotPrefab;
    public DotComponent[] dotArray;
    public bool win;
    public int totalScore;

    [SerializeField]
    CTDCurrentState gameState;
    [SerializeField]
    CTDDifficulty difficulty;
    [SerializeField]
    GameObject continueBtn, gameCanvas, overlay;
    [SerializeField]
    TMP_Text instructsTxt, ttcTxt, parasitizeTxt, scoreTxt, triesLeftTxt;
    [SerializeField]
    Vector2 minBounds, maxBounds;
    [SerializeField]
    bool wrongDotFlag;
    [SerializeField]
    float colFlashTime, numFlashTime, minDistBetweenDots, hardLim, normLim;
    [SerializeField]
    int[] maxTriesArr, numDotsArr;
    [SerializeField]
    int curIdx, scorePerDot, scorePerTry, triesLeft;

    void Update()
    {
        ConnectTheDots();
    }

    // Connect the Dots game loop
    public void ConnectTheDots()
    {
        switch(gameState)
        {
            case CTDCurrentState.Starting:
                SetupGame();
                break;
            case CTDCurrentState.Playing:
                PlayGame();
                break;
            case CTDCurrentState.Ending:
                EndGame();
                break;
            case CTDCurrentState.Intro:
            default:
                ShowIntroScreen();
                break;
        }
    }

    // Reset minigame
    public void ResetGame()
    {
        foreach (var dot in dotArray)
        {
            Destroy(dot.gameObject);
        }
        gameCanvas.SetActive(false);
        overlay.SetActive(false);
        continueBtn.SetActive(false);
        parasitizeTxt.enabled = false;
        scoreTxt.enabled = false;
        gameState = CTDCurrentState.Intro;
    }

    // Return to Game World
    public void ReturnToGameWorld()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.LoadingEnvironment);
    }

    // Show instructions for player
    void ShowIntroScreen()
    {
        if (!overlay.activeInHierarchy || !instructsTxt.enabled)
        {
            overlay.SetActive(true);
            instructsTxt.enabled = true;
            ttcTxt.enabled = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            instructsTxt.enabled = false;
            ttcTxt.enabled = false;
            overlay.SetActive(false);
            gameState = CTDCurrentState.Starting;
        }
    }

    void SetDifficulty()
    {
        float compatibility = GameManager.Instance.currentHostCompatibility;
        if (compatibility < hardLim)
        {
            difficulty = CTDDifficulty.Hard;
        }
        else if (compatibility < normLim)
        {
            difficulty = CTDDifficulty.Normal;
        }
        else
        {
            difficulty = CTDDifficulty.Easy;
        }
    }
    // Set starting values and generate dots
    void SetupGame()
    {
        SetDifficulty();
        gameCanvas.SetActive(true);
        curIdx = -1;
        triesLeft = maxTriesArr[(int)difficulty];
        triesLeftTxt.text = "Tries Left: " + triesLeft;
        dotArray = new DotComponent[numDotsArr[(int)difficulty]];
        GenerateDots(numDotsArr[(int)difficulty]);
        gameState = CTDCurrentState.Playing;
    }

    // Logic for player to connect dots
    void PlayGame()
    {
        // Game is still starting
        if (dotArray.Length > 0 && !dotArray[^1].isActive)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            wrongDotFlag = false;
        }
        if (!wrongDotFlag)
        {
            if (curIdx >= 0)
            {
                dotArray[curIdx].dragIndicator.UpdateLine(); ;
            }
            if ((curIdx < 0 && Input.GetMouseButtonDown(0)) || (curIdx >= 0 && dotArray[curIdx].dragIndicator.startPosSet && Input.GetMouseButton(0)))
            {
                CheckForDot();
            }
        }
    }

    // Calculate and show end results
    void EndGame()
    {
        if (overlay.activeInHierarchy)
        {
            return;
        }
        overlay.SetActive(true);
        parasitizeTxt.enabled = true;
        scoreTxt.enabled = true;
        continueBtn.SetActive(true);
        win = curIdx + 1 >= dotArray.Length;
        if (win)
        {
            GameManager.Instance.currentHost.Parasitized?.Invoke();
            parasitizeTxt.text = "Parasitization successful!";
        }
        else
        {
            parasitizeTxt.text = "Parasitization failed";
        }
        CalculateRes(out int score, out int bonus);
        scoreTxt.text = "";
        scoreTxt.text += "Score: " + score;
        if (triesLeft > 1)
        {
            scoreTxt.text += "\nBonus: " + bonus + "\n";
            scoreTxt.text += "Total Score: " + totalScore;
        }
    }

    // Calc score and bonus
    void CalculateRes(out int score, out int bonus)
    {
        score = (curIdx + 1) * scorePerDot;
        bonus = (triesLeft-1) * scorePerTry;
        totalScore = (triesLeft > 1) ? score + bonus : score;
        GameManager.Instance.playerPoints += totalScore;
    }

    // See if player has hit a dot
    void CheckForDot()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null)
        {
            DotComponent tempDot;
            if (tempDot = hit.transform.GetComponent<DotComponent>())
            {
                // Correct dot
                if (tempDot.number == curIdx + 2)
                {
                    // Connect end of indicator to the next dot
                    if (curIdx >= 0)
                    {
                        var curDragIndicator = dotArray[curIdx].dragIndicator;
                        curDragIndicator.SetLinePos(1, tempDot.transform.position);
                        curDragIndicator.endPosSet = true;
                    }
                    // Start drawing next indicator
                    var nextDragIndicator = tempDot.dragIndicator;
                    nextDragIndicator.lr.enabled = true;
                    nextDragIndicator.SetLinePos(0, tempDot.transform.position);
                    nextDragIndicator.SetLinePos(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    nextDragIndicator.startPosSet = true;
                    curIdx++;
                    if (curIdx == dotArray.Length-1)
                    {
                        dotArray[curIdx].dragIndicator.lr.enabled = false;
                        gameState = CTDCurrentState.Ending;
                    }
                }
                // Wrong dot
                else if (tempDot.number > curIdx + 2)
                {
                    ShowWrongDot(tempDot);
                }
            }
        }
    }

    // Logic when wrong dot is chosen
    void ShowWrongDot(DotComponent wrongDot)
    {
        if (curIdx >= 0)
        {
            dotArray[curIdx].dragIndicator.lr.enabled = false;
        }
        wrongDotFlag = true;
        triesLeft--;
        triesLeftTxt.text = "Tries Left: " + (triesLeft);
        if (triesLeft < 1)
        {

            gameState = CTDCurrentState.Ending;
        }
        StartCoroutine(wrongDot.FlashWrongColor(colFlashTime));
    }

    // Find a position for the next dot a min distance away from others
    // if no spots can be found just pick a random position
    Vector2 FindDotPosition(int index)
    {
        Vector2 dotPos = new();
        bool validDot;
        int loopCount = 0;
        do
        {
            validDot = true;
            dotPos.Set(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
            for (int i = 0; i < index; i++)
            {
                if (Vector2.Distance(dotPos, dotArray[i].transform.position) < minDistBetweenDots)
                {
                    validDot = false;
                    break;
                }
            }
        } while (validDot == false && loopCount++ < 100);
        return dotPos;
    }

    // Create all of the dots
    void GenerateDots(int numDots)
    {
        for (int i = 0; i < numDots; i++)
        {
            Vector2 position = FindDotPosition(i);
            DotComponent dot = Instantiate(dotPrefab, position, Quaternion.identity);
            dot.SetNumber(i+1);
            dotArray[i] = dot;
            //StartCoroutine(dot.FlashDotNumber(numFlashTime));
        }
        StartCoroutine(DotComponent.FlashDotNumbers(dotArray, numFlashTime));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((maxBounds.x + minBounds.x) / 2, (maxBounds.y + minBounds.y) / 2, 0),
                        new Vector3(maxBounds.x-minBounds.x, maxBounds.y-minBounds.y, 0));
    }
}
