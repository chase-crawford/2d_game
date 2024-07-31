using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Speed : MonoBehaviour
{

    public int effectRadius;
    public int effectDuration;
    public GameObject AOE;

    public bool isAOE = false;
    public ParticleSystem particles;

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAOE)
        {
            if (collision.gameObject.tag == "Ground")
            {
                GameObject aoe = Instantiate(AOE, transform.position, Quaternion.identity);
                aoe.GetComponent<Speed>().isAOE = true;
                aoe.GetComponent<CircleCollider2D>().radius = effectRadius;
                aoe.GetComponent<Light2D>().pointLightInnerRadius = effectRadius;


                ParticleSystem particle = Instantiate(particles, aoe.transform.position, Quaternion.identity);

                aoe.transform.parent = particle.transform;
                
                var main = particle.main;
                main.duration = effectDuration;

                particle.Play();

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAOE)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Add("speed");
            }

            else if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyAI>().statuses.Add("speed");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isAOE)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Remove("speed");
                other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Add("speed_timed");
            }

            else if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyAI>().statuses.Remove("speed");
                other.gameObject.GetComponent<EnemyAI>().statuses.Add("speed_timed");
            }
        }
    }
}
