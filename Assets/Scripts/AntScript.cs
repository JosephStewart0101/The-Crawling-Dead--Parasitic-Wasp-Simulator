using UnityEngine;

public class AntScript : MonoBehaviour
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    float speed = 1f;
    public float minimumSpeed = 8f;
    public float maximumSpeed = 12f;

    private void Start()
    {
        speed = Random.Range(minimumSpeed, maximumSpeed);
        if (transform.rotation.eulerAngles.z == 180)
        {
            sr.flipY = true;
        }
        Destroy(gameObject, 4); // deletes the ant 4 seconds after it's created
    }

    void FixedUpdate()
    {
        Vector2 horizontal = new Vector2 (transform.right.x, transform.right.y);

        // multiplying by Time.fixedDeltaTime ensures that the ant moves at the same speed on all devices
        rb.MovePosition(rb.position + horizontal * Time.fixedDeltaTime * speed);
    }
}
