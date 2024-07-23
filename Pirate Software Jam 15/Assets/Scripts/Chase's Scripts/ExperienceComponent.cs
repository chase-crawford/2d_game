using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceComponent : MonoBehaviour
{
    public int xp = 0;
    public int level = 1;

    private DamageComponent dmgComp;

    // Start is called before the first frame update
    void Start()
    {
        dmgComp = GetComponentInParent<DamageComponent>();
        dmgComp.onKill += UpdateXP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateXP(){
        // Add xp to counter; arbitrary since I havent set up xp values
        xp += 1;

        // Add 
        if(xp >= nextLevelXP()){
            xp -= nextLevelXP();
            level += 1;
            
        }
    }

    public int nextLevelXP(){
        return level * 10;
    }
}
