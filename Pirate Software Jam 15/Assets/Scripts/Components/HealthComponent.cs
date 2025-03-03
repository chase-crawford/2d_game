using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    //create event!
    public delegate void OnDeath();
    public event OnDeath onDeath;
    public CustomTrigger damageCollider;

    //health
    [Header("Health")]
    public float maxHealth = 100;
    public float health;

    [Header("Invulnerability")]
    public int numberOfFlashes = 5;
    public float flashDuration = .15f;
    public Color flashColor = Color.red;
    public Color regularColor = Color.white;
    private float runtimeInvulnerability;
    private bool canTakeDamage = true;

    // particles
    [Header("Particles")]
    public ParticleSystem dmgParticle;
    private ParticleSystem particleInstance;

    [Header("UI")]
    public GameObject hpBar;

    // sfx
    [Header("Audio")]
    public AudioClip[] damageSFXs;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        onDeath += Die;

        if (gameObject.tag != "Player")
            hpBar.SetActive(false);

        damageCollider.StayCollider += OnDamageColliderStay;
        damageCollider.EnterCollider += OnDamageColliderStay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Take Damage based on an attack
    public void TakeDamage(DamageComponent dmg)
    {   
        if(!canTakeDamage)
            return;

        if (!dmg.isDamageable(this.tag))
            return;

        StartCoroutine(InvulnerableFlash());

        //take damage
        health -= dmg.damage;

        UpdateHPBar();

        //Call particles
        CreateParticles(dmg.gameObject.transform.localScale);

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

        // Take Damage based on an attack
    public void TakeDamage(float damage, Vector2 attackDirection)
    {   
        StartCoroutine(InvulnerableFlash());

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

    private IEnumerator InvulnerableFlash()
    {
        // get entity sprite
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        
        // disable damage collider
        damageCollider.gameObject.SetActive(false);

        // for each flash -> change colors and wait
        for (int i=0; i<numberOfFlashes; i++)
        {
            sprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            sprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
        }

        // re-enable damage collider
        damageCollider.gameObject.SetActive(true);
    }

    public void OnDamageColliderStay(Collision2D collision)
    {
        // get damage component of other collider
        DamageComponent dmg = collision.collider.gameObject.GetComponent<DamageComponent>();

        // if other collider has damage component -> take damage    
        if (dmg != null)
        {
            TakeDamage(dmg);
        }
        else
        {
            // Debugging if damageComponent is not real
                //Debug.Log("Error: Damage Source's DamageComponent is null. Collision name: "+collision.collider.name);
                //Debug.Log("Own name: "+this.name+"\ndmg: "+dmg);
        }
    }
}
