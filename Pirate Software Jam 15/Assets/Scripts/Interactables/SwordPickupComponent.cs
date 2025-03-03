using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickupComponent : MonoBehaviour
{
    
    public string[] interactableTags;
    public AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in interactableTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<Melee>().hasSword = true;
                SoundFXManager.instance.PlaySoundClip(pickupSfx, transform, 1f);
            }
        }
    }
}
