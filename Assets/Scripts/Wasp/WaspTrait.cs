using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Scriptable Objects/Trait")]
public class WaspTrait : ScriptableObject
{
    // Color is for test purposes since traits dont have different sprites
    public Color color;
    public Sprite sprite;
    public WaspMorphology morphology;
}
