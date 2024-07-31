using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingComponent : MonoBehaviour
{
    private List<GameObject> recipes;
    private int recipeNum = 0;

    private GameObject canvas;
    private bool canCraft = false;

    private CraftingManager craftManager;
    private HerbManager herbManager;
    private PotionManager potionManager;

    void Start()
    { 
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);

        craftManager = GameObject.Find("Crafting Manager").GetComponent<CraftingManager>();
        herbManager = GameObject.Find("Herb Manager").GetComponent<HerbManager>();
        potionManager = GameObject.Find("Potion Manager").GetComponent<PotionManager>();

        recipes = craftManager.recipes;
        //UpdateRecipe();
    }

    void Update()
    {
        if (canCraft)
        {
            if (Input.GetKeyDown("e"))
            {
                recipeNum += 1;

                if (recipeNum >= recipes.Count) recipeNum = 0;

                UpdateRecipe();
            }

            else if (Input.GetKeyDown("q"))
            {
                recipeNum -= 1;

                if (recipeNum < 0) recipeNum = recipes.Count-1;

                UpdateRecipe();
            }

            else if (Input.GetKeyDown("f"))
            {
                Recipe chosenRecipe = recipes[recipeNum].GetComponent<Recipe>();
                HerbVars[] inventory = herbManager.herbs;

                bool craftable = true;
                foreach (RecipeMaterials cost in chosenRecipe.materials)
                {
                    HerbVars costHerb = herbManager.GetHerb(cost.herbType);
                    if (costHerb.inventoryNum < cost.cost)
                    {
                        craftable = false;
                        break;
                    }
                }

                //
                // CRAFTING AREA. Add Item to Inventory/Bag here
                //
                if (craftable)
                {
                    foreach (RecipeMaterials cost in chosenRecipe.materials)
                    {
                        HerbVars currentHerb = herbManager.GetHerb(cost.herbType);
                        currentHerb.inventoryNum -= cost.cost;
                        herbManager.SetHerb(currentHerb);
                    }

                    // update ui for inventory
                    GameObject.Find("Herb Inventory UI").GetComponent<HerbUIManager>().UpdateUI(herbManager);

                    // add crafted item to inventory
                    chosenRecipe.AddToInventory();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            canCraft = true;
            UpdateRecipe();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(false);
            canCraft = false;
        }
    }

    void UpdateRecipe()
    {
        Recipe previous = GetComponentInChildren<Recipe>();

        if (previous != null)
            Destroy(previous.gameObject);

        Instantiate(recipes[recipeNum], transform.GetChild(0));
    }
}
