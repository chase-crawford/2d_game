using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PotionManager : MonoBehaviour
{
    public Potions[] potionsList;

    private LobProjectile lob;

    public int potionIndex = 0;

    public GameObject potionImgPrefab;
    public GameObject amountPrefab;
    public Color Grayout;
    public int flaskScale = 10;
    public int flaskPadding = 20;
    public int betweenMargin = 5;

    private GameObject previousPotion, currentPotion, nextPotion, amount;

    void Start()
    { 
        // create ui elements
        GameObject canvas = GameObject.Find("UI");
        previousPotion = Instantiate(potionImgPrefab, canvas.transform);
        nextPotion = Instantiate(potionImgPrefab, canvas.transform);
        currentPotion = Instantiate(potionImgPrefab, canvas.transform);
        amount = Instantiate(amountPrefab, canvas.transform);


        // rotate previous and next elements for visual effect
        previousPotion.transform.Rotate(0,0,15);
        nextPotion.transform.Rotate(0,0,-15);

        // set ui elements on screen
        UpdateUI();

        UpdateUISprites();
    }

    void Update()
    {
       /* else if (Input.GetKeyDown("z"))
        {
            potionIndex -= 1;

            if (potionIndex < 0)
                potionIndex = potionsList.Length - 1;
        }
        else if (Input.GetKeyDown("x"))
        {
            potionIndex += 1;

            if (potionIndex >= potionsList.Length)
            {
                potionIndex = 0;
            }
        }*/
    }

    public void UpdateUI()
    {
        // creating flask UI element garbage

        // Get canvas, its rect, and its height
        GameObject canvas = GameObject.Find("UI");
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;


        // get each ui elements rect
        RectTransform currentRect = currentPotion.GetComponent<RectTransform>();
        RectTransform previousRect = previousPotion.GetComponent<RectTransform>();
        RectTransform nextRect = nextPotion.GetComponent<RectTransform>();
        RectTransform amountRect = amount.GetComponent<RectTransform>();

        // fix ui number's anchors
        amountRect.anchorMin = amountRect.anchorMax = new Vector2(.5f, .5f);
        amountRect.pivot = new Vector2(.5f, .5f);

        // Get uis text to set alignment
        amount.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        // Get width of each element + their vertical padding
        float uiImgWidth = flaskScale * currentRect.rect.width;
        float uiImgHeight = flaskScale * currentRect.rect.height;
        float yPadding = uiImgHeight + flaskPadding;

        // Set each element's position on screen
        currentRect.anchoredPosition = new Vector2(canvasWidth/2 - uiImgWidth*2 - flaskPadding - betweenMargin,-canvasHeight/2 + yPadding);
        previousRect.anchoredPosition = new Vector2(canvasWidth/2 - uiImgWidth*3 - flaskPadding - betweenMargin*2,-canvasHeight/2 + yPadding - 10);
        nextRect.anchoredPosition = new Vector2(canvasWidth/2 - uiImgWidth - flaskPadding, -canvasHeight/2 + yPadding - 10);
        amountRect.anchoredPosition = new Vector2(canvasWidth/2 - uiImgWidth*2 - flaskPadding - betweenMargin, -canvasHeight/2 + flaskPadding + uiImgHeight/2);

        // update elements to proper scale
        currentPotion.transform.localScale = new Vector3(flaskScale,flaskScale,1);
        previousPotion.transform.localScale = new Vector3(flaskScale,flaskScale,1);
        nextPotion.transform.localScale = new Vector3(flaskScale,flaskScale,1);
    }

    public void UpdateUISprites()
    {
        // Get index of previous and next potion
        int previousIndex = potionIndex == 0 ? potionsList.Length-1 : potionIndex-1;
        int nextIndex = potionIndex == potionsList.Length-1 ? 0 : potionIndex+1;

        // get sprites for images
        currentPotion.GetComponent<Image>().sprite = potionsList[potionIndex].Brew.GetComponent<SpriteRenderer>().sprite;
        currentPotion.GetComponent<Animator>().runtimeAnimatorController = potionsList[potionIndex].animator;

        previousPotion.GetComponent<Image>().sprite = potionsList[previousIndex].Brew.GetComponent<SpriteRenderer>().sprite;
        previousPotion.GetComponent<Animator>().runtimeAnimatorController = potionsList[previousIndex].animator;

        nextPotion.GetComponent<Image>().sprite = potionsList[nextIndex].Brew.GetComponent<SpriteRenderer>().sprite;
        nextPotion.GetComponent<Animator>().runtimeAnimatorController = potionsList[nextIndex].animator;

        // Gray out non-current options
        previousPotion.GetComponent<Image>().color = nextPotion.GetComponent<Image>().color = Grayout;

        // Update potion amount
        amount.GetComponent<Text>().text = potionsList[potionIndex].amountHeld.ToString();
    }
}