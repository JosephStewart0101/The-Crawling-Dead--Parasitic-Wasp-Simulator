using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Host Weaknesses")]

// Stores information for host weaknesses to different
// wasp traits. Information is accessed through a dictionary
public class HostWeaknesses : ScriptableObject
{
    [SerializeField]
    WaspMorphology allWaspTraits;

    public Dictionary<WaspTrait, float> dictionary;

    [SerializeField]
    float endoparasitic, ectoparasitic, paraVenom, symEndoVenom, koinobiont, idiobiont, longOvipositor, shortThickOvipositor, 
        shortThinOvipositor, smallBody, largeBody;

    public void CreateWeaknessDictionary()
    {
        dictionary = new Dictionary<WaspTrait, float>();
        float[] temp =
        {
            endoparasitic, ectoparasitic, paraVenom, symEndoVenom, koinobiont, idiobiont, longOvipositor, shortThickOvipositor,
            shortThickOvipositor, smallBody, largeBody
        };
        int i = 0;
        foreach (WaspTrait trait in allWaspTraits.traits) 
        {
            dictionary.Add(trait, temp[i++]);
        }
    }
}
