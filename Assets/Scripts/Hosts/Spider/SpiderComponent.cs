using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderComponent : HostBehaviour
{
    public IEnumerator curCoroutine;
    public WebComponent web;

    [SerializeField]
    List<Transform> waypoints;
    [SerializeField]
    Vector2 catchPos;
    [SerializeField]
    bool isRemoving;
    [SerializeField]
    [Range(0f, 1f)]
    float moveChance;
    [SerializeField]
    float idleTime;

    private void Start()
    {
        InitializeHost();
    }

    // Spider randomly decides to move to a random way point or idle
    public IEnumerator Think()
    {
        yield return new WaitForSeconds(idleTime);
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand < moveChance)
        {
            if (waypoints.Count > 0)
            {
                int i = UnityEngine.Random.Range(0, waypoints.Count);
                curDests.Add(waypoints[i].position);
                curDests.Add(spawn);
                MoveToDest(curDests[0]);
            }
            else
            {
                Debug.LogError("No waypoints");
            }
        }
        else
        {
            curCoroutine = Think();
            StartCoroutine(curCoroutine);
        }
    }

    // Move to closest point on web if not on it
    public void MoveToWeb(Vector2 catchPos)
    {
        this.catchPos = catchPos;
        isParasitizable = false;
        var col = web.GetComponent<Collider2D>();

        curDests = new()
        {
            col.ClosestPoint(transform.position)
        };
        SubscribeToDelegate(ref DestReached, MoveToCatch);
        MoveToDest(curDests[0]);
    }

    // Move to where web has been touched
    private void MoveToCatch()
    {
        DestReached -= MoveToCatch;
        curDests.Add(catchPos);
        curDests.Add(spawn);
        SubscribeToDelegate(ref DestReached, ReleaseCatch);
        MoveToDest(curDests[0]);

    }

    // Release object from web
    private void ReleaseCatch()
    {
        DestReached -= ReleaseCatch;
        web.Release();
        isParasitizable = true;
    }

    private void OnDrawGizmos()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            foreach(Transform wp in waypoints)
            {
                Gizmos.DrawWireSphere(wp.position, wpWidth);
                Gizmos.DrawLine(transform.parent.position, wp.position);
            }
        }
    }
}
