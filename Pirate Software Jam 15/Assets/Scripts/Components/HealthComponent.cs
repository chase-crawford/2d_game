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

    // particles
    public ParticleSystem dmgParticle;
    private ParticleSystem particleInstance;
    public GameObject hpBar;

    // sfx
    public AudioClip[] damageSFXs;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        onDeath += Die;

        if (gameObject.tag != "Player")
            hpBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Take Damage based on an attack
    public void TakeDamage(float damage, Vector2 attackDirection)
    {   
        //take damage
        health -= damage;

        UpdateHPBar();

        //Call particles
        CreateParticles(attackDirection);

        //play sound clip
        SoundFXManager.instance.PlayRandomSoundClip(damageSFXs, transform, 1f);

        //check for kill
        if(health <= 0){
            health = 0;
            onDeath?.Invoke();

            if (gameObject.tag == "Player")
            {
                GameManager.instance.GameOver();
            }
        }

        //Debug.Log("Damage Taken!");
    }

    public void Heal(float increase)
    {
        health += increase;

        if (health > maxHealth)
            health = maxHealth;
    }

    public void Die(){
        if (gameObject.tag == "Player")
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    private void CreateParticles(Vector2 attackDirection)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, attackDirection);

        particleInstance = Instantiate(dmgParticle, transform.position, rot);
    }

    private void UpdateHPBar()
    {
        hpBar.transform.GetChild(1).localScale = new Vector3(health/maxHealth,1,1);

        if (gameObject.tag != "Player")
        {
            if (health != maxHealth)
            {
                hpBar.SetActive(true);
            }
            else
            {
                hpBar.SetActive(false);
            }
        }
    }
}
