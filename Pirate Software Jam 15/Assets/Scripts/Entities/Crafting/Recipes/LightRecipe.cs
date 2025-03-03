using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightRecipe : Recipe
{
    public int lightAmount;

    public override void AddToInventory()
    {
        LightManager.instance.AddLight(lightAmount);
    }

}
