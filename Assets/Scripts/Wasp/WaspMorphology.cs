using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Morphology")]

// Used to group similar wasp traits
public class WaspMorphology : ScriptableObject
{
   const int SUM = 10;

    public WaspTrait[] traits;
    public string description;
    public float traitMax; // The max value a trait can have if all traits have high compatability

    private void Awake()
    {
        traitMax = (float)SUM / traits.Length;
    }
}
