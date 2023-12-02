using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaxisMovement : MonoBehaviour
{

    public bool IsTouched()
    {
        if (Input.touchCount > 0)
        {
            // Loop through all active touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Check if the touch phase is began or moved
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    // Convert the touch position to world coordinates
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    // Check if the touch position is within the sprite's bounds
                    if (GetComponent<Collider2D>().OverlapPoint(touchPosition))
                    {
                        // The sprite is being touched
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
