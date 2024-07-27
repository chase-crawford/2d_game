using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    //create event!
        public delegate void OnDeath();
        public event OnDeath onDeath;

    //health
    public float maxHealth = 100;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        onDeath += Die;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Take Damage based on an attack
    public void TakeDamage(float damage)
    {   
        //take damage
        health -= damage;

        //check for kill
        if(health <= 0){
            health = 0;
            onDeath?.Invoke();
        }

        //Debug.Log("Damage Taken!");
    }

    public void Die(){
        Destroy(gameObject);
    }
}
