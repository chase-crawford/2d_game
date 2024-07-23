using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float health = GetComponentInParent<HealthComponent>().health;
       float maxHealth = GetComponentInParent<HealthComponent>().maxHealth;

       float ratio = health/maxHealth;

       transform.localScale = new Vector3(ratio, 1, 1);
    }
}
