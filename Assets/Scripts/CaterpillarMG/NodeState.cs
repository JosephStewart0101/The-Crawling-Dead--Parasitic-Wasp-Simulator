using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeState : MonoBehaviour
{
    public bool isDecisionNode;
    public bool isFrenzyNode;

    public GameObject nodeUp;
    public GameObject nodeDown;
    public GameObject nodeLeft;
    public GameObject nodeRight;

    [SerializeField]
    private bool isActive;

    [SerializeField]
    private SpriteRenderer nodeSprite;
    [SerializeField]
    private SpriteRenderer frenzySprite;

    void Start()
    {
        if (isFrenzyNode)
        {
            frenzySprite.enabled = true;
        }
    }

    /// <summary>
    /// Used to disable the current node
    /// </summary>
    public void DisableNode()
    {
        isActive = false;
        nodeSprite.enabled = isActive;

        if (isFrenzyNode)
        {
            frenzySprite.enabled = isActive;
        }
    }

    /// <summary>
    /// Used to enable the current node
    /// </summary>
    public void EnableNode()
    {
        isActive = false;
        nodeSprite.enabled = isActive;
    }

    /// <summary>
    /// Used for debugging, print the state
    /// </summary>
    public void PrintState()
    {
        Debug.LogError("Node is in the " + isActive + " state");
    }
}
