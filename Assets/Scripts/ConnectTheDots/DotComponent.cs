using System.Collections;
using UnityEngine;
using TMPro;

enum DotChildren
{
    Sprite, Number
}

// Logic for dots in Connect the Dots minigame
public class DotComponent : MonoBehaviour
{
    public DragIndicatorScript dragIndicator;
    public bool isActive;
    public int number;

    [SerializeField]
    Color nColor, wrongColor;

    // Sets number of dot
    public void SetNumber(int number)
    {
        this.number = number;
        transform.GetChild((int)DotChildren.Number).GetComponent<TMP_Text>().text = number.ToString();
    }

    // Hides number and shows sprite
    public void HideNumber()
    {
        transform.GetChild((int)DotChildren.Number).gameObject.SetActive(false);
        transform.GetChild((int)DotChildren.Sprite).gameObject.SetActive(true);
    }

    // Hides sprite and shows number
    public void ShowNumber()
    {
        transform.GetChild((int)DotChildren.Number).gameObject.SetActive(true);
        transform.GetChild((int)DotChildren.Sprite).gameObject.SetActive(false);
    }

    // Flashes then hides dot number
    public IEnumerator FlashDotNumber(float flashTime)
    {
        ShowNumber();
        isActive = false;
        yield return new WaitForSeconds(flashTime);
        HideNumber();
        isActive = true;
    }

    // Flashes then hides all dot numbers in an array
    public static IEnumerator FlashDotNumbers(DotComponent[] dots, float flashTime)
    {
        foreach(var dot in dots)
        {
            dot.ShowNumber();
            dot.isActive = false;
            yield return new WaitForSeconds(flashTime);
            dot.HideNumber();
            dot.isActive = true;
        }
    }

    // Changes dot color for set time then chnages back
    public IEnumerator FlashWrongColor(float flashTime)
    {
        SpriteRenderer sr = transform.GetComponentInChildren<SpriteRenderer>();
        sr.color = wrongColor;
        for (float timer = flashTime; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetMouseButtonUp(0)) 
            {
                break;
            }
            yield return null;
        }
        sr.color = nColor;
    }
}
