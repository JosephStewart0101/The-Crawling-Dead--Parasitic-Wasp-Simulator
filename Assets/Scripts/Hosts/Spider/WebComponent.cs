using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebComponent : MonoBehaviour
{
    public delegate void TouchedDelegate(Vector2 pos);
    public TouchedDelegate Touched;

    [SerializeField]
    SpiderComponent spider;
    [SerializeField]
    PlayerMovement pm;
    Rigidbody2D rb;
    [SerializeField]
    float prevGS;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement temp;
        if (collision.gameObject.TryGetComponent(out temp))
        {
            pm = temp;
            rb = pm.GetComponent<Rigidbody2D>();
            pm.enabled = false;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            Touched?.Invoke(rb.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement temp))
        {
            pm.enabled = true;
            pm = null;
        }
    }

    public void Release()
    {
        if (pm != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = prevGS;
        }
        rb = null;
    }
}
