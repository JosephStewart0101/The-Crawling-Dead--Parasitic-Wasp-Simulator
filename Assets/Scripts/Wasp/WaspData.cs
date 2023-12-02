using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]

// Holds player wasp information
public class WaspData
{
    public List<WaspMorphology> allMorphologies;
    public WaspTrait largeSize, longOvi;
    
    // Trait player has for each morphology
    // Each index corresponds to a different morph
    public List<WaspTrait> traits;

    public bool hasLargeSize, hasLongOvi;

    public void UpdateTraits(WaspTrait[] newTraits)
    {
        hasLargeSize = false;
        hasLongOvi = false;
        traits = new();
        foreach (WaspTrait trait in newTraits)
        {
            traits.Add(trait);
            if (trait == largeSize)
            {
                hasLargeSize = true;
                continue;
            }
            if (trait == longOvi)
            {
                hasLongOvi = true;
                continue;
            }
        }
    }

    // Returns 0 if there is no compatability and 1 for complete compatability
    public float GetHostCompatability(HostWeaknesses weaknesses)
    {
        float compatRatio = 0f;

        foreach(WaspTrait trait in traits)
        {
            float value = weaknesses.dictionary[trait] / trait.morphology.traitMax;
            compatRatio += value > 1 ? 1 : value;
        }
        if (allMorphologies.Count == 0)
        {
            Debug.LogError("allMorphologies.Count is 0");
            return 0;
        }
        return compatRatio / allMorphologies.Count;
    }
}
