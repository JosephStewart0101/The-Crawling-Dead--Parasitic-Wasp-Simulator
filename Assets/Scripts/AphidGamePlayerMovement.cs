using UnityEngine;
using UnityEngine.SceneManagement;

public class AphidGamePlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed = 8f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FixedJoystick joystick;

    // Variables for setting Wasp Sprite
    [SerializeField] private AnimatorOverrideController shortWaspAnimC;


    private void Start()
    {
        if (!GameManager.Instance.waspData.hasLongOvi)
        {
            GetComponent<Animator>().runtimeAnimatorController = shortWaspAnimC;
        }
    }


    void Update()
    {
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;

        //Detect sprite flipping
        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Ant")
    //    {
    //        Debug.Log("Game failed!");
    //        //GameManager.Instance.ChangeState(GameManager.GameState.LoadingEnvironment);
    //        //AphidGameManager.GameOver();
    //    }

    //}
}
