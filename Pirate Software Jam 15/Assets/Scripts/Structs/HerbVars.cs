using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct HerbVars
{
    public Herb herbType;
    public Sprite sprite;
    public int inventoryNum;

    public HerbVars(Herb t, Sprite s, int i){
        herbType = t;
        sprite = s;
        inventoryNum = i;
    }
}
