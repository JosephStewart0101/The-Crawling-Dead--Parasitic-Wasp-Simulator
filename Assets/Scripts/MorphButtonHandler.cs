using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphButtonHandler : MonoBehaviour
{
    public GameObject waspMorhCanvas;
    public GameObject morphCanvas2;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch is on the object
            if (touch.phase == TouchPhase.Began && IsTouchOnObject(touch.position))
            {
                MorphCanvas2();
            }
        }

        bool IsTouchOnObject(Vector2 touchPosition)
        {
            // Convert touch position to world coordinates
            Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            // Perform a physics raycast to check if the touch is on the object
            RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition, Vector2.zero);

            // Check if the raycast hit the object
            return hit.collider != null && hit.collider.gameObject == gameObject;
        }
    }

    public void MorphCanvas2()
    {
        waspMorhCanvas.SetActive(false);

        if (!morphCanvas2.activeSelf)
        {
            morphCanvas2.SetActive(true);

        }
        else
        {
            morphCanvas2.SetActive(false);
        }
    }
}
