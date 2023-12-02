using UnityEngine;

//Determine if eggs have been touhced by player.
public class EggCollision : MonoBehaviour
{
    public bool found = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        found = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        found = true;
    }
}
