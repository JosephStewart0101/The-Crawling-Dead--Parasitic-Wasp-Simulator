using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleEggBase : MonoBehaviour
{
    GameManager gameManager;
    public HostWeaknesses weaknesses;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void StartMinigame()
    {
        gameManager.ChangeState(GameManager.GameState.DiggingMG);
    }
}
