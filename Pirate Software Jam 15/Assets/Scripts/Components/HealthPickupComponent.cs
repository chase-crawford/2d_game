using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupComponent : MonoBehaviour
{

    public float healthChange = 1f;

    public string[] interactableTags;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in interactableTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                // get object's health
                HealthComponent hpComp = other.gameObject.GetComponent<HealthComponent>();

                if (hpComp != null)
                {
                    hpComp.TakeDamage(-healthChange);
                    Destroy(gameObject);
                }
            }
        }
    }
}

