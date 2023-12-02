using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform playerTransform;

    private float cooldown = 0.5f;
    private bool cooldownCheck = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CaterpillarWarp") && cooldownCheck)
        {
            Debug.Log("Warping player from " + collision.gameObject.name);
            playerTransform.position = collision.gameObject.GetComponent<CaterpillarWarp>().GetDestination().position;
            StartCoroutine(WaitForCooldown());
        }
        else
            Debug.Log(collision.tag);
    }

    public IEnumerator WaitForCooldown()
    {
        cooldownCheck = false;
        yield return new WaitForSeconds(cooldown);
        cooldownCheck = true;
    }
}
