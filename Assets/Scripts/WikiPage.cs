using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Wiki Page")]
public class WikiPage : ScriptableObject
{
    public string title;
    [TextArea(5, 20)]
    public string description;
    public Sprite image;
}
