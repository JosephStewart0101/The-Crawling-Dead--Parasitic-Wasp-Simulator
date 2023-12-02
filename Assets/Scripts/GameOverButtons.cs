using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public TMP_Text pointsEarnedText;
    public AudioSource click;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "GameOver")
        {
            pointsEarnedText.text = $"Ponits Earned: {gameManager.playerPoints}";
        }
    }

    public void HandlePlayAgain()
    {
        gameManager.SavePoints();
        gameManager.ResetGameData();
        gameManager.ChangeState(GameManager.GameState.LoadingEnvironment);
    }
    
    public void HandleMainMenu()
    {
        //click.Play();
        gameManager.SavePoints();
        gameManager.ResetGameData();
        gameManager.ChangeState(GameManager.GameState.MainMenu);
    }
}
