using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public float speed = 5f; // move speed
    public float airSpeed = 1f;
    public float speedDampener = .9f; // acceleration/deceleration
    public float crouchSpeed = 2.5f;
    public float slideTime = 2f; // time sliding friction stays
    public float slideForce = 400f; // extra speed from sliding
    public float jumpForce = 400f; // jump height
    public int maxJumps = 1; // can make double/triple jumps


    private float slidingTime = 0;
    private int jumps;
    public List<string> statuses = new List<string>(); // process if user is sliding, jumping, etc.
    private Rigidbody2D rb; // add movement to user

    void Awake() {
        // get rb component
            rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {   // get speed multiplier
            float speed_mult = statuses.Contains("in_air") ? airSpeed : statuses.Contains("crouching") ? crouchSpeed : speed; 
        // Calculate input move force based on Input & speed multiplier
            float inputForce = Input.GetAxis("Horizontal") * speed_mult;
       
        // Calculate applied force based off current force and acceleration
            Vector2 addedForce = new Vector2((inputForce - rb.velocity.x) * speedDampener, 0);

        // if not in air or sliding -> move forward
            if (!(statuses.Contains("sliding")))
                rb.AddForce(addedForce);

        // if moving -> add moving status
            if (rb.velocity.x != 0)
            {
                if (!(statuses.Contains("moving")))
                    statuses.Add("moving");
            }
            else
                statuses.Remove("moving");
    

        // get GameObject Information
            Vector3 velocity = rb.velocity;
            Vector3 scale = transform.localScale;

        // Flip character depending on movement direction
            scale.x = velocity.x > 0 ? 1 : velocity.x < 0 ? -1 : scale.x;

        // If press crouch -> shrink down and add status
            if (Input.GetButtonDown("Crouch"))
            {
                scale.y = transform.localScale.y / 2;
                transform.position -= new Vector3(0, .5f * scale.y, 0);
                statuses.Add("crouching");

                // If moving into crouch -> start sliding and get sliding boost
                    if(velocity.x != 0)
                    {
                        slidingTime = slideTime;
                        rb.AddForce(new Vector2((velocity.x > 0 ? 1 : -1)*slideForce, 0));
                        statuses.Add("sliding");
                    }
            }

        // if sliding and slide is still initiated -> update sliding time
            if (slidingTime > 0 && statuses.Contains("sliding"))
            {
                slidingTime -= Time.deltaTime;
            }
        // stop sliding
            else
            {
                statuses.Remove("sliding");
            }
        
        // if stop crouching -> go back to regular height + stop crouching/sliding
            if (Input.GetButtonUp("Crouch"))
            {
                transform.position += new Vector3(0, .5f * scale.y, 0);
                scale.y = transform.localScale.y * 2;

                statuses.Remove("sliding");
                statuses.Remove("crouching");
            }  

        // update transform scale
            transform.localScale = scale;

        // if press jump and have jumps left -> jump
            if (Input.GetButtonDown("Jump"))
            {
                if (statuses.Contains("wallclimbing"))
                {
                    rb.AddForce(new Vector2(-350*transform.localScale.x, jumpForce));
                }
                
                else if (jumps > 0)
                {
                    rb.AddForce(new Vector2(0, jumpForce));
                    jumps--;

                    if (jumps == maxJumps-1)
                        statuses.Add("jumping");
                }
            }
        
    }

    // If touching something (needs to be changed to check for tag) -> reset jumps
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log(collision.transform.position);
            Debug.DrawLine(collision.transform.position, transform.position, Color.red, 3f);
            jumps = maxJumps;
            statuses.Remove("in_air");
            statuses.Remove("jumping");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && rb.velocity.y != 0)
        {
            statuses.Add("wallclimbing");
            rb.gravityScale = .2f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*.2f);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            statuses.Remove("wallclimbing");
            rb.gravityScale = 1;
        }
    }
}
