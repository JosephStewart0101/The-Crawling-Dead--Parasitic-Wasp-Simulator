using UnityEngine;
using TMPro;
using System.Collections.Generic;

// Handles editing and saving customimed wasp
// through the Morphology Canvas
public class WaspMorphManager : MonoBehaviour
{
    [SerializeField]
    WaspMorphology[] morphologies;
    [SerializeField]
    TMP_Text morphName;
    [SerializeField]
    SpriteRenderer spriteRenderer;  // Sprite for current trait showing
    [SerializeField]
    WaspData wd;  // Player wasp
    [SerializeField]
    WaspTrait[] selectedTraits;
    [SerializeField]
    int currentMorph, currentTrait;

    // Start is called before the first frame update
    void OnEnable()
    {
        // First time creating wasp
        if (wd.traits == null || wd.traits.Count == 0)
        {
            wd.traits = new List<WaspTrait>();
            for (int i = 0; i < morphologies.Length; i++)
            {
                wd.traits.Add(morphologies[i].traits[0]);
            }
        }
        selectedTraits = new WaspTrait[morphologies.Length];
        for (int i=0; i<selectedTraits.Length; i++)
        {
            selectedTraits[i] = wd.traits[i];
        }
        currentMorph = 0;
        SetCurrentTraitFromIndex();
        UpdateCanvas();
    }

    // Get the index of current trait from 
    // selectedTraits array
    private void SetCurrentTraitFromIndex()
    {
        currentTrait = System.Array.IndexOf(morphologies[currentMorph].traits, selectedTraits[currentMorph]);
    }

    void UpdateCanvas()
    {
        spriteRenderer.sprite = selectedTraits[currentMorph].sprite;
        spriteRenderer.color = selectedTraits[currentMorph].color; // CHANGE COLOR FOR TESTING PURPOSES
        morphName.text = morphologies[currentMorph].name;
    }

    public void NextTrait()
    {
        currentTrait = (currentTrait+1) % morphologies[currentMorph].traits.Length;
        selectedTraits[currentMorph] = morphologies[currentMorph].traits[currentTrait];
        UpdateCanvas();
    }

    public void PrevTrait() 
    { 
        if (currentTrait == 0) 
        {
            currentTrait += morphologies[currentMorph].traits.Length;
        }
        currentTrait--;
        selectedTraits[currentMorph] = morphologies[currentMorph].traits[currentTrait];
        UpdateCanvas();
    }

    public void NextMorphology()
    {
        currentMorph = (currentMorph+1) % morphologies.Length;
        SetCurrentTraitFromIndex();
        UpdateCanvas();
    }

    public void PrevMorphology()
    {
        if (currentMorph == 0)
        {
            currentMorph += morphologies.Length;
        }
        currentMorph--;
        SetCurrentTraitFromIndex();
        UpdateCanvas();
    }

    public void UpdateWasp()
    {
        wd.UpdateTraits(selectedTraits);
    }
}
