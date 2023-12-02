using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public Vector2 lowerBounds;
    public Vector2 upperBounds;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        //if (newPos.x < lowerBounds.x)
        //{
        //    newPos.x = lowerBounds.x;
        //}
        //else if (newPos.x > upperBounds.x)
        //{
        //    newPos.x = upperBounds.x;
        //}
        //if (newPos.y < lowerBounds.y)
        //{
        //    newPos.y = lowerBounds.y;
        //}
        //else if (newPos.y > upperBounds.y)
        //{
        //    newPos.y = upperBounds.y;
        //}
        transform.position = newPos;
    }
}
