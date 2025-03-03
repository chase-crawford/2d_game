using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;

    public List<GameObject> recipes;

    void Awake()
    {
        instance = this;
    }
}
