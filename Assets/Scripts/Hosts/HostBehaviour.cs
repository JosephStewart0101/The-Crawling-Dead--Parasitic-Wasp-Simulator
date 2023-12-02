using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all hosts
public class HostBehaviour : MonoBehaviour
{
    public delegate void EmptyDelegate();
    public EmptyDelegate DestReached, Parasitized;
    public delegate void MoveDelegate(Vector2 dst);
    public MoveDelegate Moving;

    public GameManager.GameState minigame;
    public HostWeaknesses weaknesses;
    public Rigidbody2D rb;
    public List<Vector2> curDests;
    public Vector2 spawn;
    public HostWikiPage page;
    public bool isParasitizable;
    public int points;
    public float compatibility, compatMin = .5f, speed, wpWidth;

    private void Start()
    {
        InitializeHost();
    }

    // Assign necessary host information
    public void InitializeHost()
    {
        weaknesses.CreateWeaknessDictionary();
        compatibility = GameManager.Instance.waspData.GetHostCompatability(weaknesses);
        Debug.Log(gameObject.name + " " + compatibility);
        spawn = transform.position;
        SubscribeToDelegate(ref DestReached, DestReachedMethod);
        SubscribeToDelegate(ref Parasitized, UnlockWikiPage);
    }

    // Move rb in a given direction
    public virtual void Move(Vector2 direction)
    {
        Moving?.Invoke(direction);
        rb.velocity = speed * direction.normalized;
    }

    // Move rb to a given destination
    public void MoveToDest(Vector2 dst)
    {
        Move(dst - rb.position);
    }

    // Stop rb
    public void StopRB()
    {
        Move(Vector2.zero);
    }

    // Always called when DestReached is invoked
    public void DestReachedMethod()
    {
        curDests.RemoveAt(0);
        StopRB();
    }

    // Called to subscribe to a delegate, avoids multiple assignments of the same method
    public static bool SubscribeToDelegate<T>(ref T dest, T myDelegate) where T : Delegate
    {
        if (dest == null || Array.IndexOf(dest.GetInvocationList(), myDelegate) < 0)
        {
            dest = (T)Delegate.Combine(dest, myDelegate);
            return true;
        }
        return false;
    }

    // Starts host minigame
    public void StartMinigame()
    {
        GameManager.Instance.currentHost = this;
        GameManager.Instance.currentHostCompatibility = compatibility;
        GameManager.Instance.ChangeState(minigame);
    }

    // Unlocks Host wiki page
    private void UnlockWikiPage()
    {
        page.UnlockPage();
        Parasitized -= UnlockWikiPage;
    }
}
