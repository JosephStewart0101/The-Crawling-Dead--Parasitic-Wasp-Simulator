using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidGameCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ends game if player touchs an ant
        if (collision.tag == "Ant")
        {
            Debug.Log("Game failed!");
            AphidGameManager.winningCondition = false;
            AphidGameManager.gameOver = true;
        }
        
        // wins game if player touches the aphid
        if (collision.tag == "Aphid")
        {
            Debug.Log("Game complete!");
            AphidGameManager.winningCondition = true;
            AphidGameManager.gameOver = true;
        }

    }
}
