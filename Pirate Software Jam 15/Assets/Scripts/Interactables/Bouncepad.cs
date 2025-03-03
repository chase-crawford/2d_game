using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncpad : MonoBehaviour
{
    public int bounces;
    public int reboundVelocity;
    public AudioClip bounceSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && rb.gameObject.tag != "Ground")
        {
            SoundFXManager.instance.PlaySoundClip(bounceSFX, transform, 1f);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, reboundVelocity);
            bounces--;

            if (bounces <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
