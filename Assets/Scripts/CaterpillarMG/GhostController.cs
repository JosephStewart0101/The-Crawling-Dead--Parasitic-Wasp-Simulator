using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints;
    int current = 0;
    public float speed = 1f;

    private void FixedUpdate()
    {
        // Haven't reached waypoint target
        if (transform.position != waypoints[current].position)
        {
            Vector2 toMove = Vector2.MoveTowards(transform.position, waypoints[current].position, speed);

            GetComponent<Rigidbody2D>().MovePosition(toMove);
        }
        // Waypoint reached
        else
        {
            current = (current + 1) % waypoints.Length;
        }
    }
}
