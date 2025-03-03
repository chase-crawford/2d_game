using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Recipe : MonoBehaviour
{
    public GameObject materialPrefab;
    public GameObject imagePrefab;
    public Sprite imageSprite;
    public RecipeMaterials[] materials;

    public float materialUiSize = .4f;
    public float craftedItemUISize = .5f;

    void Awake()
    {
        // make recipe take up full canvas size
        RectTransform bg = GetComponent<RectTransform>();
        bg.anchorMin = new Vector2(0,0);
        bg.anchorMax = new Vector2(1,1);
        bg.offsetMin = new Vector2(0,0);
        bg.offsetMax = new Vector2(0,0);

        // get height of container for positioning
        float containerHeight = bg.rect.height;

        // set padding for ui elements
        float yPos = containerHeight - materialUiSize -.05f;
        float xPadding = -(materialUiSize + .625f);

        // for each material in recipe -> make ui element
        for (int i=0; i<materials.Length; i++)
        {
            // create object
            GameObject material = Instantiate(materialPrefab, transform);

            // position image on terminal
            RectTransform sizing = material.GetComponent<RectTransform>();
            sizing.sizeDelta = new Vector2(materialUiSize,materialUiSize);
            sizing.anchorMin = new Vector2(1,0);
            sizing.anchorMax = new Vector2(1,0);
            sizing.anchoredPosition = new Vector2(xPadding,yPos);

            // get correct picture for material
            HerbManager manager = GameObject.Find("Herb Manager").GetComponent<HerbManager>();
            foreach (HerbVars herb in manager.herbs)
            {
                if (herb.herbType == materials[i].herbType) material.GetComponent<Image>().sprite = herb.sprite;
            }

            // position+size text on terminal
            RectTransform textSizing = material.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            textSizing.sizeDelta = new Vector2(1,1);
            textSizing.anchoredPosition = new Vector2(.1f, 0);
            textSizing.transform.localScale = new Vector3(.8f,.8f,1);

            // set text value to cost
            Text text = material.GetComponentInChildren<Text>();
            text.fontSize = 1;
            text.text = materials[i].cost.ToString();

            // increment y for next material
            yPos -= materialUiSize + .1f;
        }

        // create item image
        GameObject craftedItem = Instantiate(imagePrefab, transform);
        craftedItem.GetComponent<Image>().sprite = imageSprite;

        // position image on terminal
        RectTransform imgSizing = craftedItem.GetComponent<RectTransform>();
        imgSizing.sizeDelta = new Vector2(craftedItemUISize,craftedItemUISize);
        imgSizing.anchorMin = new Vector2(0,0);
        imgSizing.anchorMax = new Vector2(0,0);
        imgSizing.anchoredPosition = new Vector2(.225f+materialUiSize/2, containerHeight/2);

        //craftedItem.GetComponent<Image>().sortingOrder = 1;
    }

    //placeholder for inherited classes' inventory functionality
    public virtual void AddToInventory(){}
}
