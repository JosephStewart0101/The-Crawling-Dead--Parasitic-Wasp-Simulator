using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void ContinuePressed()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.LoadingEnvironment);
    }
}
