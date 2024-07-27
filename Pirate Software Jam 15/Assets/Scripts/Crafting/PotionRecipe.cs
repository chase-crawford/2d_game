using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionRecipe : Recipe
{

    public override void AddToInventory()
    {
        PotionManager potionManager = GameObject.Find("Potion Manager").GetComponent<PotionManager>();

        for (int i=0; i<potionManager.potionsList.Length; i++)
        {
            // get crafting item's and potion in potion list's sprites
            Sprite potionListSprite = potionManager.potionsList[i].Brew.GetComponent<SpriteRenderer>().sprite;
            Sprite recipeSprite = imageSprite;

            if (potionListSprite == recipeSprite)
            {
                potionManager.potionsList[i].amountHeld++;
                potionManager.UpdateUISprites();
                break;
            }
        }
    }

}
