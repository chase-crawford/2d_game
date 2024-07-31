using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int detectionRadius = 3;
    public float aggroTimer = 10f;
    public float speed = 3f;
    public float jumpForce = 400f;
    public int maxJumps = 1;
    public Color gizmoColor = Color.white;

    public bool canJump = true;
    public bool canWalk = true;
    public bool canFly = true;

    private GameObject player = null;
    private float aggroTime = 0f;
    private Rigidbody2D rb;
    private int jumps = 0;
    public List<string> statuses = new List<string>();

    public AudioClip[] jumpSFX;
    public AudioClip alertSFX;


    void Awake()
    {
        statuses.Add("Patroling");

        // get rigidbody for movement
            rb = GetComponent<Rigidbody2D>();

        // set detection radius to public var input
            CircleCollider2D detection = GetComponent<CircleCollider2D>();
            detection.radius = detectionRadius;

        // if flying -> cut off gravity
            if (canFly)
            {
                rb.gravityScale = 0;
                rb.drag = .5f;
                statuses.Add("flying");
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inMenu)
            return;

        if (aggroTime > 0)
        {
            aggroTime -= Time.deltaTime;

            // get direction between player and enemy
            Vector2 distance = player.transform.position - transform.position;

            // move horizontally
                if ((canWalk || canFly) && !statuses.Contains("in_air"))
                {
                    //Debug.Log("Trying to Move");
                    Vector2 horizontalForce = new Vector2(distance.x, 0).normalized * speed;
                    horizontalForce.x -= rb.velocity.x;
                    rb.AddForce(horizontalForce);

                    // update looking direction
                        Vector3 scale = transform.localScale;
                        scale.x = rb.velocity.x > 0 ? Mathf.Abs(scale.x) : rb.velocity.x < 0 ? -Mathf.Abs(scale.x) : scale.x;
                        transform.localScale = scale;
                }

            // jump
                if (canJump)
                {
                    if (distance.y > 1 && jumps > 0 && distance.x < 2)
                    {
                        // play jump sfx
                        SoundFXManager.instance.PlayRandomSoundClip(jumpSFX, transform, .5f);

                        //Debug.Log("Trying to Jump");
                        rb.AddForce(new Vector2(0, jumpForce));
                        jumps--;
                        statuses.Add("in_air");
                    }
                }


            // flying
                if (canFly)
                {
                    //Debug.Log("Trying to Fly");
                    Vector2 verticalForce = new Vector2(0, distance.y).normalized * speed/16;
                    verticalForce.x -= rb.velocity.y;
                    rb.AddForce(verticalForce);
                }
        }
        else
        {
            player = null;

            if (statuses.Contains("Aggroing"))
            {
                statuses.Remove("Aggroing");
                statuses.Add("Patroling");
            }
        }
    }


    // When enemy enters detection path, check for aggro
        void OnTriggerStay2D(Collider2D other)
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

                        if (statuses.Contains("Patroling"))
                        {
                            // play alert sfx the first time. Dear god it was an awful sound if I didnt do it that way. Might have gotten tinitus from that. - C
                            SoundFXManager.instance.PlaySoundClip(alertSFX, transform, .75f);

                            statuses.Remove("Patroling");
                            statuses.Add("Aggroing");
                        }
                    }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Touched something", other.gameObject);
                if (other.CompareTag("Ground"))
                {
                    //Debug.Log("Touched Ground");
                    jumps = maxJumps;
                    statuses.Remove("in_air");
                }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, detectionRadius);
        }
}
