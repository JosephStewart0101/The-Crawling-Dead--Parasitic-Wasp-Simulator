using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to determine when the player is touching the flight button. 
//And for detecting nearby hosts to activate the parasitization button.
//Also handles the activation of parasitaztion button
public class ButtonDetection : MonoBehaviour
{
    public PlayerMovement pm;
    public HostSpawner spawner;
    public GameObject parasitizeButton;
    public AudioSource audio;
    public AudioSource fly;

    public float parasitizationDistance = 3.5f;
    private GameObject closestHost;

    // Update is called once per frame
    void Update()
    {
        if (spawner != null && parasitizeButton != null)
        {
            CheckFlightButton();
            FindNearbyHosts();
        }
    }

    private void CheckFlightButton()
    {
        pm.isFlying = false; // Assume not flying by default

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            Vector2 touchPosition = touch.position;

            // Convert touch position to world coordinates
            Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            Collider2D hit = Physics2D.OverlapPoint(touchWorldPosition);

            if (hit != null && hit.CompareTag("FlightButton"))
            {
                fly.Play();
                pm.isFlying = true; // If the touch is within the button's collider, set to flying
                pm.flying?.Invoke();
                //Debug.Log("Flying " + pm.isFlying);
                return; // No need to check further touches
            }
        }
        pm.notFlying?.Invoke();
    }


    public void HandleParasitizeButton()
    {
        audio.Play();
        pm.parasitize?.Invoke();
        string hostName = ProcessString(closestHost.name);
        if (closestHost != null)
        {
            pm.currentHost = closestHost.GetComponent<HostBehaviour>();
            if (pm.currentHost == null)
            {
                pm.currentHost = closestHost.GetComponentInChildren<HostBehaviour>();
            }
        }
        //Debug.Log("hostname: " + hostName);
        GameManager.Instance.DeleteHost(hostName);
    }

    public void FindNearbyHosts()
    {
        foreach (GameObject host in spawner.hosts)
        {
            if (host != null)
            {
                Vector3 playerPosition = pm.transform.position;
                float distance = Vector3.Distance(host.transform.position, playerPosition);

                var hostB = host.GetComponent<HostBehaviour>();
                if (hostB == null)
                {
                    hostB = host.GetComponentInChildren<HostBehaviour>();
                    distance = Vector3.Distance(hostB.transform.position, playerPosition);
                }
                if (distance < parasitizationDistance &&
                    hostB.isParasitizable &&
                    hostB.compatibility > hostB.compatMin)
                {
                    parasitizeButton.SetActive(true);
                    closestHost = host;
                    break;
                }
                else
                {
                    parasitizeButton.SetActive(false);
                }
            }
        }
    }

    private string ProcessString(string input)
    {
        // Check if the input string is not null and has more than 7 characters
        if (!string.IsNullOrEmpty(input) && input.Length > 7)
        {
            char firstCharLower = char.ToLower(input[0]);
            string processedString = firstCharLower + input.Substring(1, input.Length - 8);

            return processedString;
        }
        else
        {
            return input;
        }
    }
}