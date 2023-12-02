using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Import the UnityEngine.UI namespace

//This script handles the animation triggers and populates the wiki based on page number
public class SetPages : MonoBehaviour
{
    //[Serializable]
    //public struct WikiPage
    //{
    //    public string title;
    //    [TextArea(5, 20)]
    //    public string description;
    //    public Sprite image;
    //}

    public GameObject infoCanvas;
    public TMP_Text infoTitle;
    public TMP_Text infoDescription;
    public SpriteRenderer infoImage; // Reference to the Image component

    private int pageNum = 0;
    public WikiPage[] wikiPages;
    public string[] WikiTitles;
    [TextArea(5, 20)]
    public string[] WikiDescriptions;
    public Sprite[] WikiImages; // Array of sprites to use as images
    public AudioSource prevAudio;
    public AudioSource nextAudio;

    private void Start()
    {
        InitializePages();
    }

    public void BookIsOpen()
    {
        infoCanvas.SetActive(true);
        UpdatePage(0);
    }

    public void BookIsClosed()
    {
        infoCanvas.SetActive(false);
    }

    public void NextPage()
    {
        nextAudio.Play();
        if (pageNum >= wikiPages.Length-1)
        {
            return;
        }
        pageNum++;
        UpdatePage(pageNum);
    }


    public void PrevPage()
    {
        prevAudio.Play();
        if (pageNum <= 0)
        {
            return;
        }
        pageNum--;
        UpdatePage(pageNum);
    }

    private void InitializePages()
    {
        foreach(var page in wikiPages)
        {
            if (page is HostWikiPage hostPage)
            {
                if (PlayerPrefs.GetInt(hostPage.pageName, -1) > 0)
                {
                    hostPage.unlocked = true;
                    continue;
                }
                hostPage.unlocked = false;
            }
        }
    }

    private void UpdatePage(int pageNum)
    {
        if ((wikiPages[pageNum] is HostWikiPage page) && !page.unlocked)
        {
            infoTitle.text = "???";
            infoDescription.text = "?????";
            infoImage.color = Color.black;
            infoImage.sprite = wikiPages[pageNum].image;
            return;
        }
        infoTitle.text = wikiPages[pageNum].title;
        infoDescription.text = wikiPages[pageNum].description;
        infoImage.color = Color.white;
        infoImage.sprite = wikiPages[pageNum].image; // Change the image sprite
    }
}
