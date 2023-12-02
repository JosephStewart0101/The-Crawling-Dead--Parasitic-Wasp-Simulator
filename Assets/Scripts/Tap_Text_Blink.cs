using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_Text_Blink : MonoBehaviour
{
    public float timer;
    public GameObject tap_text;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = .5f;
            if (tap_text.activeSelf) {
                tap_text.SetActive(false);
            }
            else
            {
                tap_text.SetActive(true);
            }
        }
    }
}
