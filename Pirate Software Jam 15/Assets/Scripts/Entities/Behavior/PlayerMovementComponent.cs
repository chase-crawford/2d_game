using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public float speed = 5f; // move speed
    public float airSpeed = 4f;
    public float speedDampener = .9f; // acceleration/deceleration
    public float crouchSpeed = 2.5f;
    public float slideTime = 2f; // time sliding friction stays
    public float slideVelocity = 7; // extra speed from sliding
    public float jumpVelocity = 9f; // jump height
    public int maxJumps = 1; // can make double/triple jumps


    private float slidingTime = 0;
    public int jumps;
    public List<string> statuses = new List<string>(); // process if user is sliding, jumping, etc.
    public Animator animator;
    private Rigidbody2D rb; // add movement to user
    
    private float speedTime = 5;
    private float currentSpeedTime;

    public AudioClip[] jumpSFX;

    void Awake() {
        // get rb component
            rb = GetComponent<Rigidbody2D>(); 

            jumps = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inMenu)
            return;

        // start timer for temp speed
        if (statuses.Contains("speed_timed"))
        {
            currentSpeedTime = speedTime;

            statuses.Remove("speed_timed");
        }

        else
        {
            currentSpeedTime -= Time.deltaTime;
        }


        // get speed multiplier
            float speed_mult = statuses.Contains("in_air") ? airSpeed : statuses.Contains("crouching") ? crouchSpeed : speed; 

        // check if in speed
            if (statuses.Contains("speed") || currentSpeedTime > 0)
            {
                speed_mult += 5; // yes magic number but i dont fucking care it is 2 AM and im hungry even though I ate my pizza. Suck my nuts harder. - C
            }

        // Calculate input move force based on Input & speed multiplier
            float inputForce = Input.GetAxis("Horizontal") * speed_mult;
       
        // Calculate applied force based off current force and acceleration
            Vector2 addedForce = new Vector2((inputForce - rb.linearVelocity.x) * speedDampener, 0);

        // if not in air or sliding -> move forward
            if (!(statuses.Contains("sliding")))
                rb.AddForce(addedForce);

        // if moving -> add moving status
            if (rb.linearVelocity.x != 0)
            {
                if (!(statuses.Contains("moving")))
                {
                    statuses.Add("moving");
                }
            }
            else
            {
                statuses.Remove("moving");
            }

    

        // get GameObject Information
            Vector3 velocity = rb.linearVelocity;
            Vector3 scale = transform.localScale;

        // Flip character depending on movement direction
            if (!statuses.Contains("attacking"))
                scale.x = inputForce > 0 ? Mathf.Abs(scale.x) : inputForce < 0 ? -Mathf.Abs(scale.x) : scale.x;

        // If press crouch -> shrink down and add status
            if (Input.GetButtonDown("Crouch"))
            {
                scale.y = 5*transform.localScale.y / 6;
                transform.position -= new Vector3(0, 1/6 * scale.y, 0);
                statuses.Add("crouching");

                // If moving into crouch -> start sliding and get sliding boost
                    if(Mathf.Abs(velocity.x) > 2 && slidingTime <= 0)
                    {
                        slidingTime = slideTime;
                        rb.linearVelocity = new Vector2(transform.localScale.x*slideVelocity, rb.linearVelocity.y);
                        statuses.Add("sliding");
                    }
            }


        // if sliding and slide is still initiated -> update sliding time
            if (slidingTime > 0)
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
                transform.position += new Vector3(0, 1/6f * scale.y, 0);
                scale.y = transform.localScale.y * 6/5;

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
                    rb.AddForce(new Vector2(-350*transform.localScale.x, jumpVelocity));
                }
                
                else if (jumps > 0)
                {
                    float yVel = jumpVelocity / (rb.linearVelocity.y < -1 ? 1.5f : 1);

                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, yVel);
                    jumps--;

                    if (jumps == maxJumps-1)
                        statuses.Add("jumping");

                    if (!statuses.Contains("in_air"))
                        statuses.Add("in_air");

                    SoundFXManager.instance.PlayRandomSoundClip(jumpSFX, transform, 1f);
                }
            }
        
        // update animation parameters
            animator.SetBool("isMoving", statuses.Contains("moving"));
            animator.SetBool("isCrouching", statuses.Contains("crouching"));
            animator.SetBool("inAir", statuses.Contains("in_air"));
    }

    // If touching something (needs to be changed to check for tag) -> reset jumps
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log(collision.transform.position);
            Debug.DrawLine(collision.transform.position, transform.position, Color.red, 3f);
            jumps = maxJumps;
            statuses.Remove("in_air");
            statuses.Remove("jumping");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && rb.linearVelocity.y != 0)
        {
            statuses.Add("wallclimbing");
            rb.gravityScale = .2f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y*.2f);
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
