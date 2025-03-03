using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Potions
{
    public int Position;
    public string Batch;
    public GameObject Brew;
    public int amountHeld;
    public RuntimeAnimatorController animator;
    public PotionType potionType;
}