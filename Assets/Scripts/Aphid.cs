using UnityEngine;
using UnityEngine.SceneManagement;

public class Aphid : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
        Debug.Log("Game complete!");
        GameManager.Instance.ChangeState(GameManager.GameState.LoadingEnvironment);

    }
}
