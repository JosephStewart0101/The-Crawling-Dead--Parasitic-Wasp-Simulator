using JetBrains.Annotations;
//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MorphologyMngr : MonoBehaviour
{
    GameManager gameManager;
    public GameObject Canvas;
    public Transform CanvasTransform;
    public Dictionary<string, int> morphologies;
    private Dictionary<string, int> activeMorphologies;

    public Dictionary<string, int> lockedTraits;
    public GameObject traitLock;
    public Dictionary<string, GameObject> traitLocks = new Dictionary<string, GameObject>();
    public GameObject purchaseCanvas;

    public UnityEngine.UI.Image smallBody;
    public UnityEngine.UI.Image largeBody;
    public UnityEngine.UI.Image koinobiont;
    public UnityEngine.UI.Image idiobiont;
    public UnityEngine.UI.Image longOvipositor;
    public UnityEngine.UI.Image shortThinOvipositor;
    public UnityEngine.UI.Image endoparasitic;
    public UnityEngine.UI.Image ectoprasitic;
    public UnityEngine.UI.Image paralyticVenom;
    public UnityEngine.UI.Image symbioticVirus;
    public Sprite[] sprites;

    [Header("Wasp Data")]
    public List<WaspMorphology> allMorphologies;
    public List<WaspTrait> morphTraits;
    public WaspTrait smallBodyTrait, largeBodyTrait, koinobiontTrait, idiobiontTrait, longOviTrait, shortOviTrait,
                     endoparasiticTrait, ectoparasiticTrait, paralyticVenomTrait, symbioticVirusTrait;
    public SpriteRenderer waspSprite;
    public Sprite longOviWaspSprite, shortOviWaspSprite;
    public Vector2 waspScale;
    public float largeMultiplier;
    [Space(20)]

    private Dictionary<string, float[]> traitPositions = new Dictionary<string, float[]>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        waspScale = waspSprite.transform.localScale;
        SetUpSprites();
        UpdateWaspSprite();
        transform.root.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SetUpSprites();
    }

    public void InitalizeVariables()
    {
        morphologies = gameManager.GetMorphologies();
        lockedTraits = gameManager.GetLockedTraits();
        activeMorphologies = morphologies;

        gameManager.PrintMorphologies();
        gameManager.PrintLockedTraits();
        morphTraits = new();
    }

    public void ClearLocks()
    {
        foreach (var pair in traitLocks)
        {
            GameObject temp = pair.Value;
            Destroy(temp);
        }
        traitLocks.Clear();
    }

    public void SetUpSprites()
    {
        InitalizeVariables();

        if (traitLocks.Count > 0)
        {
            ClearLocks();
        }

        int i = 0;
        foreach(var pair in morphologies)
        {
            if (pair.Value == 1)
            {
                ActivateSprite(i, pair.Key);
            }
            i++;
        }

        i = 0;
        foreach(var pair in lockedTraits)
        {
            if (pair.Value == 0)
            {
                CreateLock(pair.Key);
            }
            i++;
        }
        GameManager.Instance.waspData.UpdateTraits(morphTraits.ToArray());
        UpdateWaspSprite();
    }

    private void ActivateSprite(int i, string name)
    {
        switch (name)
        {
            case "SmallBody":
                smallBody.sprite = sprites[(i * 2)];
                morphTraits.Add(smallBodyTrait);
                break;
            case "LargeBody":
                largeBody.sprite = sprites[(i * 2)];
                morphTraits.Add(largeBodyTrait);
                break;
            case "Koinobiont":
                koinobiont.sprite = sprites[(i * 2)];
                morphTraits.Add(koinobiontTrait);
                break;
            case "Idiobiont":
                idiobiont.sprite = sprites[(i * 2)];
                morphTraits.Add(idiobiontTrait);
                break;
            case "LongOvipositor":
                longOvipositor.sprite = sprites[(i * 2)];
                morphTraits.Add(longOviTrait);
                break;
            case "ShortThinOvipositor":
                shortThinOvipositor.sprite = sprites[(i * 2)];
                morphTraits.Add(shortOviTrait);
                break;
            case "Endoparasitic":
                endoparasitic.sprite = sprites[(i * 2)];
                morphTraits.Add(endoparasiticTrait);
                break;
            case "Ectoparasitic":
                ectoprasitic.sprite = sprites[(i * 2)];
                morphTraits.Add(ectoparasiticTrait);
                break;
            case "ParalyticVenom":
                paralyticVenom.sprite = sprites[(i * 2)];
                morphTraits.Add(paralyticVenomTrait);
                break;
            case "SymbioticVirus":
                symbioticVirus.sprite = sprites[(i * 2)];
                morphTraits.Add(symbioticVirusTrait);
                break;
        }
    }

    private void CreateLock(string name)
    {
        string newName = ConvertName(name);
        GameObject gameObject = GameObject.Find(newName);
        UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
        RectTransform imageTransform = image.rectTransform;
        
        Vector3 spawnPosition = new Vector3(imageTransform.anchoredPosition.x, imageTransform.anchoredPosition.y, 0f);
        Quaternion spawnRotation = Quaternion.identity;
        
        GameObject tLock = Instantiate(traitLock, spawnPosition, spawnRotation);
        tLock.name = newName.Substring(0, newName.Length - 6) + "Lock";

        tLock.transform.SetParent(CanvasTransform);

        traitLocks.Add(newName, tLock);
    }

    private string ConvertName(string name)
    {
        string modified = name.Substring(2) + "Button";
        return modified;
    }

    private void PrintMorpholgies(Dictionary<string, int> x)
    {
        foreach (var pair in x)
        {
            Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
        }
    }

    public void PressedSmallBody()
    {
        string name = "SmallBody";
        GameObject smallBodyLock = GameObject.Find(name+"Lock");
        if (smallBodyLock == null)
        {
            if (activeMorphologies[name] == 1)
            {
                return;
            }
            else if (activeMorphologies["LargeBody"] == 1)
            {
                ToggleImage(largeBody, sprites[2], sprites[3], "LargeBody", largeBodyTrait);
                ToggleImage(smallBody, sprites[0], sprites[1], name, smallBodyTrait);
            }
        }
        else
        {
            HandleLockPress(name, smallBodyLock, 0);
        }
    }

    public void PressedLargeBody()
    {
        string name = "LargeBody";
        GameObject largeBodyLock = GameObject.Find(name + "Lock");
        GameObject smallBodyLock = GameObject.Find("SmallBody");
        if (largeBodyLock == null)
        {
            if (smallBodyLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["SmallBody"] == 1)
                {
                    ToggleImage(smallBody, sprites[0], sprites[1], "SmallBody", smallBodyTrait);
                    ToggleImage(largeBody, sprites[2], sprites[3], name, largeBodyTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, largeBodyLock, 2);
        }
    }

    public void PressedKoinobiont()
    {
        string name = "Koinobiont";
        GameObject koinobiontLock = GameObject.Find(name + "Lock");
        GameObject idiobiontLock = GameObject.Find("IdiobiontLock");
        if (koinobiontLock == null)
        {
            if (idiobiontLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["Idiobiont"] == 1)
                {
                    ToggleImage(koinobiont, sprites[4], sprites[5], name, koinobiontTrait);
                    ToggleImage(idiobiont, sprites[6], sprites[7], "Idiobiont", idiobiontTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, koinobiontLock, 4);
        }
    }

    public void PressedIdiobiont()
    {
        string name = "Idiobiont";
        GameObject idiobiontLock = GameObject.Find(name + "Lock");
        GameObject koinobiontLock = GameObject.Find("KoinobiontLock");
        if (idiobiontLock == null)
        {
            if (koinobiontLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["Koinobiont"] == 1)
                {
                    ToggleImage(koinobiont, sprites[4], sprites[5], "Koinobiont", koinobiontTrait);
                    ToggleImage(idiobiont, sprites[6], sprites[7], name, idiobiontTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, idiobiontLock, 6);
        }
    }

    public void PressedLongOvipositor()
    {
        string name = "LongOvipositor";
        GameObject longOvipositorLock = GameObject.Find(name + "Lock");
        GameObject shortThinOvipositorLock = GameObject.Find("ShortThinOvipositorLock");
        if (longOvipositorLock == null)
        {
            if (shortThinOvipositorLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["ShortThinOvipositor"] == 1)
                {
                    ToggleImage(shortThinOvipositor, sprites[10], sprites[11], "ShortThinOvipositor", shortOviTrait);
                    ToggleImage(longOvipositor, sprites[8], sprites[9], name, longOviTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, longOvipositorLock, 8);
        }
    }

    public void PressedShortThinOvipositor()
    {
        string name = "ShortThinOvipositor";
        GameObject shortThinOvipositorLock = GameObject.Find(name + "Lock");
        GameObject longOvipositorLock = GameObject.Find("LongOvipositorLock");
        if (shortThinOvipositorLock == null)
        {
            if (longOvipositorLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["LongOvipositor"] == 1)
                {
                    ToggleImage(longOvipositor, sprites[8], sprites[9], "LongOvipositor", longOviTrait);
                    ToggleImage(shortThinOvipositor, sprites[10], sprites[11], name, shortOviTrait);
                }
            }
        }
        else
        {
            HandleLockPress(name, shortThinOvipositorLock, 10);
        }
    }

    public void PressedEndoparasitic()
    {
        string name = "Endoparasitic";
        GameObject endoparasiticLock = GameObject.Find(name + "Lock");
        GameObject ectoparasiticLock = GameObject.Find("EctoparasiticLock");
        if (endoparasiticLock == null)
        {
            if(ectoparasiticLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["Ectoparasitic"] == 1)
                {
                    ToggleImage(ectoprasitic, sprites[14], sprites[15], "Ectoparasitic", ectoparasiticTrait);
                    ToggleImage(endoparasitic, sprites[12], sprites[13], name, endoparasiticTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, endoparasiticLock, 12);
        }
    }

    public void PressedEctoparasitic()
    {
        string name = "Ectoparasitic";
        GameObject ectoparasiticLock = GameObject.Find(name + "Lock");
        GameObject endoparasiticLock = GameObject.Find("EndoparasiticLock");
        if (ectoparasiticLock == null)
        {
            if (endoparasiticLock == null)
            {
                if (activeMorphologies[name] == 1)
                {
                    return;
                }
                else if (activeMorphologies["Endoparasitic"] == 1)
                {
                    ToggleImage(endoparasitic, sprites[12], sprites[13], "Endoparasitic", endoparasiticTrait);
                    ToggleImage(ectoprasitic, sprites[14], sprites[15], name, ectoparasiticTrait);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            HandleLockPress(name, ectoparasiticLock, 14);
        }
    }

    public void PressedParalyticVenom()
    {
        string name = "ParalyticVenom";
        GameObject paralyticVenomLock = GameObject.Find(name + "Lock");
        if (paralyticVenomLock == null)
        {
            ToggleImage(paralyticVenom, sprites[16], sprites[17], name, paralyticVenomTrait);
        }
        else
        {
            HandleLockPress(name, paralyticVenomLock, 16);
        }
    }

    public void PressedSymbioticVirus()
    {
        string name = "SymbioticVirus";
        GameObject symbioticVirusLock = GameObject.Find(name + "Lock");
        if (symbioticVirusLock == null)
        {
            ToggleImage(symbioticVirus, sprites[18], sprites[19], name, symbioticVirusTrait);
        }
        else
        {
            HandleLockPress(name, symbioticVirusLock, 18);
        }
    }

    private void ToggleImage(UnityEngine.UI.Image image, Sprite activeSprite, Sprite inactiveSprite, string morphType, WaspTrait trait)
    {
        if (image.sprite == activeSprite)
        {
            // Change to inactive when pressed
            image.sprite = inactiveSprite;
            activeMorphologies[morphType] = 0;
            morphTraits.Remove(trait);
        }
        else
        {
            // Change to active
            image.sprite = activeSprite;
            activeMorphologies[morphType] = 1;
            morphTraits.Add(trait);
        }
        gameManager.SetMorphologies(activeMorphologies);
        SetUpSprites();
    }

    public void UpdateWaspSprite()
    {
        if (gameManager.waspData.hasLongOvi)
        {
            waspSprite.sprite = longOviWaspSprite;
        }
        else
        {
            waspSprite.sprite = shortOviWaspSprite;
        }
        if (gameManager.waspData.hasLargeSize)
        {
            // Increase Y scale by half of what X scale is increased by
            waspSprite.transform.localScale = Vector3.one * (waspScale * largeMultiplier);
        }
        else
        {
            waspSprite.transform.localScale = Vector3.one * waspScale;
        }
    }

    private void HandleLockPress(string name, GameObject tlock, int spriteNum)
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject temp = Instantiate(purchaseCanvas, spawnPosition, spawnRotation);
        PurchaseButtons tempScript = temp.GetComponent<PurchaseButtons>();
        tempScript.SetTraitImage(sprites[spriteNum]);
        tempScript.traitName = name;
        tempScript.lockName = "L_" + name;
    }

    public void Exit()
    {
        Canvas.SetActive(false);
    }

}
