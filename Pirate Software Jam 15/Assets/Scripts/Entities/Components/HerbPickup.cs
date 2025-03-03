using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbPickup : MonoBehaviour
{
    public Herb herbType;
    public AudioClip pickupSfx;

    void Start()
    {
        HerbManager manager = GameObject.Find("Herb Manager").GetComponent<HerbManager>();

        foreach(HerbVars herb in manager.herbs)
        {
            if (herbType == herb.herbType)
            {
                GetComponent<SpriteRenderer>().sprite = herb.sprite;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            SoundFXManager.instance.PlaySoundClip(pickupSfx, transform, 1f);

            Destroy(gameObject);

            // get object's movement
            HerbManager manager = GameObject.Find("Herb Manager").GetComponent<HerbManager>();


            if (manager != null)
            {
                for (int i=0; i<manager.herbs.Length; i++)
                {
                    if (manager.herbs[i].herbType == herbType)
                    {
                        // add 1 to herb count
                        manager.herbs[i].inventoryNum++;

                        // update ui for inventory
                        GameObject.Find("Herb Inventory UI").GetComponent<HerbUIManager>().UpdateUI(manager);
                    }
                }
            }
        }
    }
}
