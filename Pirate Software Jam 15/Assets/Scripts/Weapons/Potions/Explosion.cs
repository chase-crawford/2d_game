using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public int radius = 1;
    public ParticleSystem explosionParticles;
    public GameObject explosionObject;
    public AudioClip explosionSFX;
    public float explosionTime = .1f;

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag != "Player")
        {
            Explode();
        }

    }

    void Explode()
    {
        // Play SFX
        SoundFXManager.instance.PlaySoundClip(explosionSFX, transform, 1f);

        // Play Particle Effect
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        /*// get list of enemies in damage radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        // Check if explosion hit any enemies
        foreach (Collider2D hit in hits)
        {   
            // if hit enemy -> damage them
            if (hit.gameObject.tag == "Enemy" && !hit.isTrigger)
            {
                hit.gameObject.GetComponent<HealthComponent>().TakeDamage(damage, Vector2.right);
            }
        }*/

        Destroy(gameObject);
    }

    private IEnumerator ExplosionCollider()
    {
        GameObject explo = Instantiate(explosionObject, transform.position, Quaternion.identity);
        explo.GetComponent<DamageComponent>().damage = this.damage;

        yield return new WaitForSeconds(explosionTime);

        Destroy(explo);
    }
}
