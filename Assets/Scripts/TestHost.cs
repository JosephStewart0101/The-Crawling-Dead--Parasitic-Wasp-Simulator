using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHost : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject currentObject;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D colllsion)
    {
        if (colllsion.gameObject.name == "Player")
        {
            currentObject.SetActive(false);
            gameManager.ChangeState(GameManager.GameState.ConnectTheDotsMG);
        }
    }
}
