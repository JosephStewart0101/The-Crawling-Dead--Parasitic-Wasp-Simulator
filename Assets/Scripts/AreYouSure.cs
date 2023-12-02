using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreYouSure : MonoBehaviour
{
    public GameObject Canvas;
    public MorphologyMngr morphMngr;
    public TMP_Text DNAtext;

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        GameManager.Instance.InitalizeLockedTraits();
        GameManager.Instance.InitalizePlayerPoints();
        GameManager.Instance.InitalizeWaspMorphologyies();
        GameManager.Instance.InitializeWikiPages();
        PlayerPrefs.Save();
        DNAtext.text = GameManager.Instance.GetPlayerPoints().ToString();
        Close();
    }

    public void Close()
    {
        Canvas.SetActive(!Canvas.activeSelf);
    }
}
