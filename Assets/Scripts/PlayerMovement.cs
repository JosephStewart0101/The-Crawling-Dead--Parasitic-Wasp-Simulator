using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public delegate void BoolDelegate(bool tof);
    public BoolDelegate updateGrounded;
    public delegate void EmptyDelegate();
    public EmptyDelegate flying, notFlying, parasitize;
    public delegate void MoveDelegate(Vector2 dir);
    public MoveDelegate move;
    private GameObject currentPlatform;
    
    public HostBehaviour currentHost;

    private float horizontal;
    private float vertical;
    public float speed = 8f;
    public float jumpingPower = 4f;
    public float largeSizeScale = 1.5f;
    private bool isFacingRight = true;
    public bool isFlying = false;
    public bool isGrounded;

    public bool caterpillar = false;

    [SerializeField] Collider2D col;
    [SerializeField] LayerMask groundMask;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FixedJoystick joystick;

    // Variables for setting wasp anim controller
    [SerializeField] AnimatorOverrideController shortWaspAnimC;

    private void Start()
    {
        if (!GameManager.Instance.waspData.hasLongOvi)
        {
            GetComponent<Animator>().runtimeAnimatorController = shortWaspAnimC;
        }
        if (GameManager.Instance.waspData.hasLargeSize)
        {
            transform.localScale *= largeSizeScale;
        }
    }

    void Update()
    {
        //Player movement for inside UNITY EDITOR
        /*
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        */
		
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;

        PassThroughPlatform();
        
        //Detect sprite flipping
        Flip();
    }

	private void FixedUpdate()
    {
        move?.Invoke(new Vector2(horizontal, vertical));
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (caterpillar)
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            if (isFlying)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }
    }

    private void Flip(){
		if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckForGround())
        {
            isGrounded = true;
            updateGrounded?.Invoke(true);
        }

        if(collision != null)
        {
            if (collision.gameObject.CompareTag("OneWayPlatform"))
            {
                currentPlatform = collision.gameObject;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If not colliding with anything
        if (!CheckForGround())
        {
            isGrounded = false;
            updateGrounded?.Invoke(false);
        }

    }

    private bool CheckForGround()
    {
        var cf = new ContactFilter2D();
        cf.SetLayerMask(groundMask);
        var colArr = new Collider2D[1];
        return col.OverlapCollider(cf, colArr) != 0;
    }

    private IEnumerator ReactivatePlatform()
    {
        yield return new WaitForSeconds(0.5f);
        currentPlatform.SetActive(true);
    }

    private void PassThroughPlatform()
    {
        if(currentPlatform != null && vertical < -0.5f)
        {
            currentPlatform.SetActive(false);
            StartCoroutine(ReactivatePlatform());
        }
    }
}