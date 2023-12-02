using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCatPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogError("Collided with " + other);

        if (other.gameObject.name.StartsWith("Player"))
        {
            Debug.LogError("KillCatPlayer::OnTriggerEnter2D = Player");
        }
        if (other.gameObject.name.StartsWith("PlayerHitbox"))
        {
            Debug.LogError("KillCatPlayer::OnTriggerEnter2D = PlayerHitbox");
        }
    }
}
