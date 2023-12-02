using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField]
    private bool debugging = false;
    SceneLoader sceneLoader;
    public GameState State { get; private set; }

    //Game variables
    public Vector2 playerPos;
    public int playerPoints; // The points earned in the game round. Will be reset after each round.
    public List<GameObject> spawnedHosts = new List<GameObject>();
    public Dictionary<string, Vector3> spawnedHostsData = new Dictionary<string, Vector3>();
    public HostBehaviour currentHost;
    public WaspData waspData;
    public int parasitizationAttempts = 3;
    public int currentParaAttempts;
    public int traitCost = 500;
    public float currentHostCompatibility;

    // Initialize the state
    void Start()
    {
        sceneLoader = SceneLoader.Instance;

        //Initalize player data
        InitalizePlayerPoints();
        InitalizeWaspMorphologyies();
        InitalizeLockedTraits();
        InitializeWikiPages();
        //FreeMoney();
        //testing();

        // Initial state (e.g., TitleCard)
        ChangeState(GameState.TitleCard);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;

        if (debugging)
            Debug.Log("Changing GameState to " + newState);

        switch (newState)
        {
            case GameState.MainMenu:
                MainMenu(newState);
                break;
            case GameState.TitleCard:
                LoadTitleCard(newState);
                break;
            case GameState.DiggingMG:
                //LoadDiggingMG(newState);
                LoadMinigame("DiggingMG");
                break;
            case GameState.LoadingEnvironment:
                LoadEnvironment(newState);
                break;
            case GameState.ConnectTheDotsMG:
                LoadMinigame("ConnectTheDotsMG");
                break;
            case GameState.GameOver:
                GameOver(newState);
                break;
            case GameState.CaterpillarMG:
                LoadMinigame("CaterpillarMG");
                break;
            case GameState.AphidMG:
                LoadMinigame("AphidGame");
                break;
        }
    }

    public void FindLoadedState()
    {
        if (debugging)
            Debug.LogError("FindLoadedState: Started search for loaded state");
        switch (State)
        {
            case GameState.MainMenu:
                ChangeState(GameState.MainMenu);
                break;
            case GameState.LoadingEnvironment:
                ChangeState(GameState.LoadingEnvironment);
                break;
            case GameState.DiggingMG:
                ChangeState(GameState.DiggingMG);
                break;
            case GameState.TitleCard:
                ChangeState(GameState.TitleCard);
                break;
            case GameState.ConnectTheDotsMG:
                ChangeState(GameState.ConnectTheDotsMG);
                break;
            case GameState.GameOver:
                ChangeState(GameState.GameOver);
                break;
            case GameState.CaterpillarMG:
                ChangeState(GameState.CaterpillarMG);
                break;
            case GameState.AphidMG:
                ChangeState(GameState.AphidMG);
                break;
        }
        if (debugging)
            Debug.LogError("FindLoadedState: Error! Couldn't find loaded state!!!");
    }

    private void MainMenu(GameState currentState)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Main_Menu");
        //Debug.Log("Loading Main Menu");
        //CheckPlayerPoins();
        PrintMorphologies();
        playerPos = new Vector2(0, -21);
    }

    private void LoadTitleCard(GameState currentState)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Title_Card");
        Debug.Log("Loading Title Card");
    }

    private void LoadMinigame(string name)
    {
        SavePlayerPos();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name);
    }

    private void LoadDiggingMG(GameState currentState)
    {
        SavePlayerPos();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("DiggingMG");
    }

    public void LoadEnvironment(GameState currentState)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameWorld");
    }

    private void ConnectTheDotsMG(GameState currentState)
    {
        SavePlayerPos();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("ConnectTheDotsMG");
    }

    private void GameOver(GameState gameState)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameOver");
    }

    public void SavePlayerPos()
    {
        GameObject player = GameObject.Find("Player");
        Transform playerTransform = player.transform;
        playerPos = playerTransform.position;
        Debug.Log("Saving: " + playerPos);
    }

    public void ResetGameData()
    {
        spawnedHosts.Clear();
        spawnedHostsData.Clear();
        playerPos = new Vector3(-1.78f, -21.11f, 0);
        playerPoints = 0;
        currentParaAttempts = 0;
    }

    public void SavePoints()
    {
        int currentValue = PlayerPrefs.GetInt("Points", -1);
        currentValue += playerPoints;
        PlayerPrefs.SetInt("Points", currentValue);
        PlayerPrefs.Save();
        Debug.Log("Saved " + currentValue + " points");
    }

    private void CheckPlayerPoins()
    {
        int currentValue = PlayerPrefs.GetInt("Points", -1);
        Debug.Log("Player points: " + currentValue);
    }

    //Resets player points to 0
    private void ResetPlayerPoints()
    {
        PlayerPrefs.SetInt("Points", 0);
        PlayerPrefs.Save();
    }

    public int GetPlayerPoints()
    {
        int points = PlayerPrefs.GetInt("Points", -1);
        return points;
    }

    public void InitalizeWaspMorphologyies()
    {
        int smallBody = PlayerPrefs.GetInt("SmallBody", -1);
        if (smallBody == -1)
        {
            PlayerPrefs.SetInt("SmallBody", 0);
            PlayerPrefs.SetInt("LargeBody", 1);
            PlayerPrefs.SetInt("Koinobiont", 1);
            PlayerPrefs.SetInt("Idiobiont", 0);
            PlayerPrefs.SetInt("LongOvipositor", 1);
            PlayerPrefs.SetInt("ShortThinOvipositor", 0);
            PlayerPrefs.SetInt("Endoparasitic", 1);
            PlayerPrefs.SetInt("Ectoparasitic", 0);
            PlayerPrefs.SetInt("ParalyticVenom", 0);
            PlayerPrefs.SetInt("SymbioticVirus", 0);
            PlayerPrefs.Save();
        }
    }

    public void InitalizePlayerPoints()
    {
        int points = PlayerPrefs.GetInt("Points", -1);
        if (points == -1)
        {
            PlayerPrefs.SetInt("Points", 0);
            PlayerPrefs.Save();
        }
    }

    public void PrintMorphologies()
    {
        int smallBody = PlayerPrefs.GetInt("SmallBody", -1);
        int largeBody = PlayerPrefs.GetInt("LargeBody", -1);
        int koinobiont = PlayerPrefs.GetInt("Koinobiont", -1);
        int idiobiont = PlayerPrefs.GetInt("Idiobiont", -1);
        int longOvipositor = PlayerPrefs.GetInt("LongOvipositor", -1);
        int shortThinOvipositor = PlayerPrefs.GetInt("ShortThinOvipositor", -1);
        int endoparasitic = PlayerPrefs.GetInt("Endoparasitic", -1);
        int ectoparasitic = PlayerPrefs.GetInt("Ectoparasitic", -1);
        int paralyticVenom = PlayerPrefs.GetInt("ParalyticVenom", -1);
        int symbioticVirus = PlayerPrefs.GetInt("SymbioticVirus", -1);

        Debug.Log("Smallbody: " + smallBody + "\n" +
                  "Largebody: " + largeBody + "\n" +
                  "Koinobiont: " + koinobiont + "\n" +
                  "Idiobiont: " + idiobiont + "\n" +
                  "Long Ovipositor: " + longOvipositor + "\n" +
                  "Short Thin Ovipsoitor: " + shortThinOvipositor + "\n" +
                  "Endoparasitic: " + endoparasitic + "\n" +
                  "Ectoparasitic: " + ectoparasitic + "\n" +
                  "Paralytic Venom: " + paralyticVenom + "\n" +
                  "Symbiotic Virus: " + symbioticVirus + "\n"
                 );

    }

    public void SetMorphologies(Dictionary<string, int> morphs)
    {
        foreach (var pair in morphs)
        {
            PlayerPrefs.SetInt(pair.Key, pair.Value);
        }
        PlayerPrefs.Save();
    }

    public Dictionary<string, int> GetMorphologies()
    {
        var morphs = new Dictionary<string, int>();

        morphs["SmallBody"] = PlayerPrefs.GetInt("SmallBody", -1);
        morphs["LargeBody"] = PlayerPrefs.GetInt("LargeBody", -1);
        morphs["Koinobiont"] = PlayerPrefs.GetInt("Koinobiont", -1);
        morphs["Idiobiont"] = PlayerPrefs.GetInt("Idiobiont", -1);
        morphs["LongOvipositor"] = PlayerPrefs.GetInt("LongOvipositor", -1);
        morphs["ShortThinOvipositor"] = PlayerPrefs.GetInt("ShortThinOvipositor", -1);
        morphs["Endoparasitic"] = PlayerPrefs.GetInt("Endoparasitic", -1);
        morphs["Ectoparasitic"] = PlayerPrefs.GetInt("Ectoparasitic", -1);
        morphs["ParalyticVenom"] = PlayerPrefs.GetInt("ParalyticVenom", -1);
        morphs["SymbioticVirus"] = PlayerPrefs.GetInt("SymbioticVirus", -1);

        return morphs;
    }

    public void InitalizeLockedTraits()
    {
        int check = PlayerPrefs.GetInt("L_SmallBody", -1);

        if (check == -1)
        {
            PlayerPrefs.SetInt("L_SmallBody", 0);
            PlayerPrefs.SetInt("L_LargeBody", 1);
            PlayerPrefs.SetInt("L_Koinobiont", 1);
            PlayerPrefs.SetInt("L_Idiobiont", 0);
            PlayerPrefs.SetInt("L_LongOvipositor", 1);
            PlayerPrefs.SetInt("L_ShortThinOvipositor", 0);
            PlayerPrefs.SetInt("L_Endoparasitic", 1);
            PlayerPrefs.SetInt("L_Ectoparasitic", 0);
            PlayerPrefs.SetInt("L_ParalyticVenom", 0);
            PlayerPrefs.SetInt("L_SymbioticVirus", 0);
            PlayerPrefs.Save();
        }
    }

    public void PrintLockedTraits()
    {
        int L_smallBody = PlayerPrefs.GetInt("L_SmallBody", -1);
        int L_largeBody = PlayerPrefs.GetInt("L_LargeBody", -1);
        int L_koinobiont = PlayerPrefs.GetInt("L_Koinobiont", -1);
        int L_idiobiont = PlayerPrefs.GetInt("L_Idiobiont", -1);
        int L_longOvipositor = PlayerPrefs.GetInt("L_LongOvipositor", -1);
        int L_shortThinOvipositor = PlayerPrefs.GetInt("L_ShortThinOvipositor", -1);
        int L_endoparasitic = PlayerPrefs.GetInt("L_Endoparasitic", -1);
        int L_ectoparasitic = PlayerPrefs.GetInt("L_Ectoparasitic", -1);
        int L_paralyticVenom = PlayerPrefs.GetInt("L_ParalyticVenom", -1);
        int L_symbioticVirus = PlayerPrefs.GetInt("L_SymbioticVirus", -1);

        Debug.Log("L_Smallbody: " + L_smallBody + "\n" +
                  "L_Largebody: " + L_largeBody + "\n" +
                  "L_Koinobiont: " + L_koinobiont + "\n" +
                  "L_Idiobiont: " + L_idiobiont + "\n" +
                  "L_Long Ovipositor: " + L_longOvipositor + "\n" +
                  "L_Short Thin Ovipsoitor: " + L_shortThinOvipositor + "\n" +
                  "L_Endoparasitic: " + L_endoparasitic + "\n" +
                  "L_Ectoparasitic: " + L_ectoparasitic + "\n" +
                  "L_Paralytic Venom: " + L_paralyticVenom + "\n" +
                  "L_Symbiotic Virus: " + L_symbioticVirus + "\n"
                 );
    }

    public Dictionary<string, int> GetLockedTraits()
    {
        var locked = new Dictionary<string, int>();

        locked["L_SmallBody"] = PlayerPrefs.GetInt("L_SmallBody", -1);
        locked["L_LargeBody"] = PlayerPrefs.GetInt("L_LargeBody", -1);
        locked["L_Koinobiont"] = PlayerPrefs.GetInt("L_Koinobiont", -1);
        locked["L_Idiobiont"] = PlayerPrefs.GetInt("L_Idiobiont", -1);
        locked["L_LongOvipositor"] = PlayerPrefs.GetInt("L_LongOvipositor", -1);
        locked["L_ShortThinOvipositor"] = PlayerPrefs.GetInt("L_ShortThinOvipositor", -1);
        locked["L_Endoparasitic"] = PlayerPrefs.GetInt("L_Endoparasitic", -1);
        locked["L_Ectoparasitic"] = PlayerPrefs.GetInt("L_Ectoparasitic", -1);
        locked["L_ParalyticVenom"] = PlayerPrefs.GetInt("L_ParalyticVenom", -1);
        locked["L_SymbioticVirus"] = PlayerPrefs.GetInt("L_SymbioticVirus", -1);

        return locked;
    }

    private void SetPlayerPoints(int points)
    {
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.Save();
    }

    public void BuyTrait(string traitName, string lockName)
    {
        int points = GetPlayerPoints();
        if (points > traitCost)
        {
            points -= traitCost;
            SetPlayerPoints(points);
            PlayerPrefs.SetInt(lockName, 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Not enough points");
        }
    }

    public void InitializeWikiPages()
    {
        int aphidPage = PlayerPrefs.GetInt("AphidPage", -1);
        if (aphidPage == -1)
        {
            PlayerPrefs.SetInt("AphidPage", 0);
            PlayerPrefs.SetInt("BeetleEggsPage", 0);
            PlayerPrefs.SetInt("CaterpillarPage", 0);
            PlayerPrefs.SetInt("SpiderPage", 0);
            PlayerPrefs.Save();
        }
    }

    public void testing()
    {
        PlayerPrefs.SetInt("L_SmallBody", 0);
        PlayerPrefs.SetInt("L_LargeBody", 1);
        PlayerPrefs.SetInt("L_Koinobiont", 1);
        PlayerPrefs.SetInt("L_Idiobiont", 0);
        PlayerPrefs.SetInt("L_LongOvipositor", 1);
        PlayerPrefs.SetInt("L_ShortThinOvipositor", 0);
        PlayerPrefs.SetInt("L_Endoparasitic", 1);
        PlayerPrefs.SetInt("L_Ectoparasitic", 0);
        PlayerPrefs.SetInt("L_ParalyticVenom", 0);
        PlayerPrefs.SetInt("L_SymbioticVirus", 0);

        PlayerPrefs.SetInt("SmallBody", 0);
        PlayerPrefs.SetInt("LargeBody", 1);
        PlayerPrefs.SetInt("Koinobiont", 1);
        PlayerPrefs.SetInt("Idiobiont", 0);
        PlayerPrefs.SetInt("LongOvipositor", 1);
        PlayerPrefs.SetInt("ShortThinOvipositor", 0);
        PlayerPrefs.SetInt("Endoparasitic", 1);
        PlayerPrefs.SetInt("Ectoparasitic", 0);
        PlayerPrefs.SetInt("ParalyticVenom", 0);
        PlayerPrefs.SetInt("SymbioticVirus", 0);

        PlayerPrefs.Save();
    }

    public void DeleteHost(string hostKey)
    {
        if (spawnedHostsData.ContainsKey(hostKey))
        {
            spawnedHostsData.Remove(hostKey);
            Console.WriteLine($"Host with key '{hostKey}' deleted.");
        }
        else
        {
            Console.WriteLine($"Host with key '{hostKey}' not found.");
        }
    }

    public void FreeMoney()
    {
        PlayerPrefs.SetInt("Points", 10000);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Enum for game states.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        TitleCard,
        DiggingMG,
        LoadingEnvironment,
        ConnectTheDotsMG,
        GameOver,
        CaterpillarMG,
        AphidMG,
    }
}
