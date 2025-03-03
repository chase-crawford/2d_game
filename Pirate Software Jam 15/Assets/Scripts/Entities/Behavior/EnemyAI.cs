using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private CustomTrigger detectionTrigger;
    [SerializeField] private int detectionRadius = 3;
    [SerializeField] private float aggroTimer = 10f;
    [SerializeField] private Color gizmoColor = Color.white;
    private float aggroTime = 0f;

    [Header("Behaviour")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canStrafe = true;
    [SerializeField] private bool canFly = true;

    [Header("Audio")]
    [SerializeField] private AudioClip alertSFX;


    private GameObject player = null;
    private Rigidbody2D rb;
    private MovementComponent move;

    void Awake()
    {
        // update detection functionality & radius
            detectionTrigger.StayTrigger += CheckForPlayer;
            detectionTrigger.GetComponent<CircleCollider2D>().radius = detectionRadius;

        // get rigidbody for movement
            rb = GetComponent<Rigidbody2D>();
            move = GetComponent<MovementComponent>();

        // set detection radius to public var input
            CircleCollider2D detection = GetComponent<CircleCollider2D>();
            detection.radius = detectionRadius;

        // if flying -> cut off gravity
        // also, give drag to stop flyer from flying after lose aggro
            if (canFly)
            {
                rb.gravityScale = 0;
                rb.linearDamping = .5f;
                move.AddStatus("flying");
            }

        move.AddStatus("Patroling");
    }

    // Update is called once per frame
    void Update()
    {
        // pause game if in menu
        if (GameManager.instance.inMenu)
            return;

        // if still aggroed to player -> move
        if (aggroTime > 0)
        {
            aggroTime -= Time.deltaTime;

            // get direction between player and enemy
                Vector2 distance = player.transform.position - transform.position;
                //Debug.DrawLine(transform.position, transform.position + (Vector3)distance.normalized, Color.red, 1f);

            // strafe AI
                if (canStrafe)
                {
                    move.Strafe(distance.normalized.x);

                    // update looking direction
                        Vector3 scale = transform.localScale;
                        scale.x = rb.linearVelocity.x > 0 ? Mathf.Abs(scale.x) : rb.linearVelocity.x < 0 ? -Mathf.Abs(scale.x) : scale.x;
                        transform.localScale = scale;
                }

            // jump AI
                if (canJump)
                {
                    if (distance.y > 1 && distance.x < 2)
                    {
                        move.Jump();
                    }
                }


            // flying AI
                if (canFly)
                {
                    // fly toward player
                        move.Fly(distance);
                }
        }
        // if lost player -> go pack to patrolling
        else
        {
            player = null;

            if (move.HasStatus("Aggroing"))
            {
                move.RemoveStatus("Aggroing");
                move.AddStatus("Patroling");
            }
        }
    }


    // When enemy enters detection path -> check for aggro
        void CheckForPlayer(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // if no objects between player and enemy && enemy facing player -> aggro
                    Vector3 direction = transform.position - other.gameObject.transform.position;

                    if (!(Physics2D.Linecast((Vector2)transform.position, (Vector2)other.gameObject.transform.position, LayerMask.GetMask("Ground"))) && direction.x / transform.localScale.x < 0 && direction.magnitude <= detectionRadius)
                    {
                        Debug.DrawLine(transform.position, other.gameObject.transform.position, Color.white, 3f);
                        player = other.gameObject;
                        aggroTime = aggroTimer;

                        if (move.HasStatus("Patroling"))
                        {
                            // play alert sfx the first time. Dear god it was an awful sound if I didnt do it that way. Might have gotten tinitus from that. - C
                            SoundFXManager.instance.PlaySoundClip(alertSFX, transform, 1);

                            move.RemoveStatus("Patroling");
                            move.AddStatus("Aggroing");
                        }
                    }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, detectionRadius);
        }
}

// Refactored Code Dump
        /*void OnTriggerStay2D(Collider2D other)
        {
            
        }*/

        /*void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Touched something", other.gameObject);
                if (other.CompareTag("Ground"))
                {
                    //Debug.Log("Touched Ground");
                    jumps = maxJumps;
                    move.RemoveStatus("in_air");
                }
        }*/
