using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : MonoBehaviour
{
    GameManager gameManager;
    public Sprite longOvi;
    public Sprite shortThinOvi;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        player = GameObject.Find("Player");
        LoadPlayerPostition();
        LoadPlayerSprite();
    }

    private void LoadPlayerPostition()
    {
        Transform playerTransform = player.transform;
        if(playerTransform != null )
        {
            playerTransform.position = gameManager.playerPos;
        }
        else
        {
            Debug.Log("Player Transform null");
        }
    }

    private void LoadPlayerSprite()
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        var morphs = gameManager.GetMorphologies();
        Debug.Log("long "+ morphs["LongOvipositor"]);
        Debug.Log("short " + morphs["ShortThinOvipositor"]);
        if ( morphs != null)
        {
            if (morphs["LongOvipositor"] == 1)
            {
                spriteRenderer.sprite = longOvi;
            }
            else
            {
                spriteRenderer.sprite = shortThinOvi;
            }
        }
    }
}
