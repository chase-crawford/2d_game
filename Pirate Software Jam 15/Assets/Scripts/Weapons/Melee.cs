using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float MeleeSpeed;

    [SerializeField] private float damage;

    float timeUntilMelee;

    private void Update()
    {
        if (timeUntilMelee <= 0f) 
        {
            if(Input.GetKey(KeyCode.Mouse1))
            {
            anim.SetTrigger("Attack");
            timeUntilMelee = MeleeSpeed;
            }
        }
        else
        {
            timeUntilMelee -=Time.deltaTime;
        }
    }
}
