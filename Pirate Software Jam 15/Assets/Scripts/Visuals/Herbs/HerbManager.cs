using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HerbManager : MonoBehaviour
{
    public HerbVars[] herbs;

    public HerbVars GetHerb(Herb type)
    {
        foreach (HerbVars herb in herbs)
        {
            if (herb.herbType == type)
                return herb;
        }

        return new HerbVars(0,null,0);
    }

    public void SetHerb(HerbVars updated)
    {
        for (int i=0; i<herbs.Length; i++)
        {
            if (herbs[i].herbType == updated.herbType)
            {
                herbs[i] = updated;
                break;
            }
        }
    }
}
