using System.Collections.Generic;
using UnityEngine;

public class CaterpillarComponent : HostBehaviour
{
    [SerializeField]
    LayerMask plantMask, playerMask;
    [SerializeField]
    List<Vector2> waypoints;
    [SerializeField]
    float awarenessRad;

    private void Start()
    {
        InitializeHost();
    }

    // Scan for player
    public void Scan()
    {
        var temp = Physics2D.OverlapCircle(transform.position, awarenessRad, playerMask);
        if ( temp != null)
        {
            Flee(temp.bounds.center);
        }
    }

    // Move caterpillar to tip of leaf to prepare for drop
    void Flee(Vector2 dangerPos)
    {
        if (waypoints == null || waypoints.Count <= 1)
        {
            Debug.LogError("No waypoints for " + this);
            return;
        }
        // Flee away from danger
        var dist1 = Vector2.Distance(dangerPos, waypoints[0]);
        var dist2 = Vector2.Distance(dangerPos, waypoints[1]);
        curDests.Add(dist1 > dist2 ? waypoints[0] : waypoints[1]);

        SubscribeToDelegate(ref DestReached, Drop);
        MoveToDest(curDests[0]);
    }

    // Fall off plant
    void Drop()
    {
        isParasitizable = false;
        LayerMask excludeLayers = plantMask + playerMask;
        rb.excludeLayers = excludeLayers;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit ground after dropping from plant
        if (collision.gameObject.layer != Mathf.Log(plantMask.value, 2) &&
            isParasitizable == false)
        {
            Destroy(gameObject);
        }
        
        // On plant
        if (collision.gameObject.layer == Mathf.Log(plantMask.value, 2))
        {
            var colBounds = collision.collider.bounds;
            var fleepoint1 = new Vector2(colBounds.max.x, transform.position.y);
            var fleepoint2 = new Vector2(colBounds.min.x, transform.position.y);
            waypoints.Add(fleepoint1);
            waypoints.Add(fleepoint2);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Leaving plant
        if (collision.gameObject.layer == Mathf.Log(plantMask.value, 2))
        {
            waypoints = null;
        }
    }

    // Show awareness radius and fleepoint
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, awarenessRad);
        if (waypoints != null && waypoints.Count > 0)
        {
            foreach (var wp in waypoints)
            {
                if (wp != null)
                {
                    Gizmos.DrawWireSphere(wp, wpWidth);
                    Gizmos.DrawLine(transform.root.position, wp);
                }
            }
        }
    }
}
