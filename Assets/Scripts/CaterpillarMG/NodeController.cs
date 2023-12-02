using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public GameObject player;

    public GameObject scoreTextUpdater;

    public ScoreInt scoreInt;

    private CaterpillarGameManager caterpillarGameManager;

    public List<GameObject> listOfNodes = new List<GameObject>();

    int NodeCounter;

    private void Start()
    {
        NodeCounter = listOfNodes.Count;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        caterpillarGameManager = CaterpillarGameManager.Instance;

        //Debug.LogError("Collided with " + other);
        //Debug.LogError("GameObject name: " + other.gameObject.name);
        // Player passed over the node
        if (other.gameObject.name.StartsWith("Node"))
        {
            SpriteRenderer nodeSprite = other.gameObject.GetComponentInChildren<SpriteRenderer>();
            // Node is enabled and should be disabled, points given
            if (nodeSprite != null)
            {
                if (nodeSprite.enabled)
                {
                    // Check if is a frenzy node
                    if (other.gameObject.GetComponent<NodeState>().isFrenzyNode)
                    {
                        Debug.LogError("NodeController: Just collected a frenzy node");
                        caterpillarGameManager.RaisePlayerFrenzyEvent();
                    }

                    //Debug.LogError("Disabling node");
                    other.gameObject.GetComponent<NodeState>().DisableNode();   // Disables both sprites
                    //nodeSprite.enabled = false;
                    scoreInt.score++;
                    ScoreTextUpdater value = scoreTextUpdater.GetComponent<ScoreTextUpdater>();
                    value.OnScoreUpdate?.Invoke();
                    NodeCounter--;
                    if (NodeCounter == 0)
                    {
                        GameManager.Instance.currentHost.Parasitized?.Invoke();
                        caterpillarGameManager.SetPlayerWinLose(true);
                        caterpillarGameManager.RaisePlayerDeathEvent();
                    }
                }
            }
        }
    }
}
