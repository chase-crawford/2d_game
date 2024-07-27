using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPickupComponent : MonoBehaviour
{
    public int jumpChange = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // get object's movement
            PlayerMovementComponent movement = other.gameObject.GetComponent<PlayerMovementComponent>();


            if (movement != null)
            {
                movement.maxJumps += jumpChange;
                movement.jumps += jumpChange;
                Destroy(gameObject);
            }
        }
    }
}
