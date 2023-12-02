using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseButtons : MonoBehaviour
{

    public string traitName;
    public string lockName;
    public SpriteRenderer traitSprite;
    GameManager gameManager;
    MorphologyMngr morphMngr;

    private void Update()
    {
        DestroyOnTouch();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        morphMngr = FindAnyObjectByType<MorphologyMngr>();
    }

    public void Yes()
    {
        gameManager.BuyTrait(traitName, lockName);
        morphMngr.SetUpSprites();
        gameManager.PrintMorphologies();
        gameManager.PrintLockedTraits();
        Destroy(gameObject);
        GameObject pointCounter = GameObject.Find("DNACount");
        if (pointCounter != null)
        {
            TMP_Text pointText = pointCounter.GetComponent<TMP_Text>();
            pointText.text = gameManager.GetPlayerPoints().ToString();
        }
    }

    public void No()
    {
        Destroy(gameObject);
    }

    public void DestroyOnTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;

                // Convert touch position to world coordinates
                Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10));

                // Perform a raycast and check if the canvas is hit
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

                bool canvasHit = false;

                foreach (var hit in hits)
                {
                    if (hit.collider.CompareTag("PurchaseTrait"))
                    {
                        // The canvas was hit, so set a flag
                        canvasHit = true;
                        break;
                    }
                }

                if (!canvasHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetTraitImage(Sprite image)
    {
        traitSprite.sprite = image;
    }
}
