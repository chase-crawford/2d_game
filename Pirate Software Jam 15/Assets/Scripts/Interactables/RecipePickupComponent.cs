using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePickupComponent : MonoBehaviour
{
    public GameObject recipe;

    public string[] interactableTags;
    public AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in interactableTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                Destroy(gameObject);
                CraftingManager.instance.recipes.Add(recipe);
                SoundFXManager.instance.PlaySoundClip(pickupSfx, transform, 1f);
            }
        }
    }
}
