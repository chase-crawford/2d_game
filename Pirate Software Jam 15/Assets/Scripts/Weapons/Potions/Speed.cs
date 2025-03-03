using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Speed : MonoBehaviour
{

    public int effectRadius;
    public int effectDuration;
    [SerializeField] float linger = 3;
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

    // Hey Chase! It's 1:22 AM Chase. To-Do when you get to this:
        // When you take damage you still collide with enemies
        // When you enter haste it doesnt add speed status, mainly because it gets the damage collider.

        // Maybe work with collider priority, new layers, tags, etc? No clue.
        // I messed with the main colliders a bit too much and it was a headache so you get it now... Have Fun!


    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAOE)
        {
            MovementComponent move = other.gameObject.GetComponent<MovementComponent>();
            Debug.Log(other.gameObject.name);

            if (move != null)
                move.AddStatus("speed");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isAOE)
        {
            MovementComponent move = other.gameObject.GetComponent<MovementComponent>();

            if (move != null)
            {
                move.SpeedUp(linger);
            }
        }
    }
}

// Refactored Code
    /*if (other.gameObject.tag == "Player")
    {
        other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Add("speed");
    }

    else if (other.gameObject.tag == "Enemy")
    {
        other.gameObject.GetComponent<EnemyAI>().statuses.Add("speed");
    }*/

    /*if (other.gameObject.tag == "Player")
    {
        other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Remove("speed");
        other.gameObject.GetComponent<PlayerMovementComponent>().statuses.Add("speed_timed");
    }

    else if (other.gameObject.tag == "Enemy")
    {
        other.gameObject.GetComponent<EnemyAI>().statuses.Remove("speed");
        other.gameObject.GetComponent<EnemyAI>().statuses.Add("speed_timed");
    }*/