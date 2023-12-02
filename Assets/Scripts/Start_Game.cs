using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Game : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void StartGame()
    {
        // Load Player data here

        // Change the game state to MainMenu
        gameManager.ChangeState(GameManager.GameState.MainMenu);
    }

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // This block of code will run when the touch begins (tap)
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}
