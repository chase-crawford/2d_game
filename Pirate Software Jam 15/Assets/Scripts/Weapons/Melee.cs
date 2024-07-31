using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float MeleeSpeed = 1;

    [SerializeField] private float damage;

    [SerializeField] private AudioClip swordSfx;

    float timeUntilMelee;

    private PlayerMovementComponent movement;

    public bool hasSword;

    private void Awake()
    {
        movement = GetComponent<PlayerMovementComponent>();
    }

    private void Update()
    {
        if (GameManager.instance.inMenu || !hasSword)
            return;

        if (timeUntilMelee <= 0f) 
        {
            movement.statuses.Remove("attacking");
            anim.gameObject.SetActive(false);

            if(Input.GetKey(KeyCode.Mouse1))
            {
                SoundFXManager.instance.PlaySoundClip(swordSfx, transform, 1f);

                movement.statuses.Add("attacking");

                anim.gameObject.SetActive(true);
                anim.SetTrigger("Attack");
                timeUntilMelee = MeleeSpeed;
            }
        }
        else
        {
            timeUntilMelee -=Time.deltaTime;
        }
    }
}
