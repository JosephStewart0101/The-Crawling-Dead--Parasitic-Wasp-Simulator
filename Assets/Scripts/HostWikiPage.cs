using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Host Wiki Page")]
public class HostWikiPage : WikiPage
{
    public bool unlocked;
    public string pageName;

    public void UnlockPage()
    {
        unlocked = true;
        PlayerPrefs.SetInt(pageName, 1);
        PlayerPrefs.Save();
    }
}
