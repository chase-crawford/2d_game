using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [Header("Strafing")]
    [SerializeField] private float strafeSpeed = 5f; // move speed
    [SerializeField] private float airSpeed = 4f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float speedDampener = .9f; // acceleration/deceleration

    [Header("Sliding")]
    [SerializeField] private float slideTime = 2f; // time sliding friction stays
    [SerializeField] private float slideVelocity = 7; // extra speed from sliding
    [SerializeField] private float SlideSpeedThreshold = 2;

    [Header("Jumping")]
    [SerializeField] private float jumpVelocity = 9f; // jump height
    [SerializeField] private int maxJumps = 1; // can make double/triple jumps
    [SerializeField] private CustomTrigger? jumpTrigger; 
    [SerializeField] private float coyoteTime = .3f;

    [Header("Flying")]
    [SerializeField] private float verticalFlySpeed = 1;
    [SerializeField] private float horizontalFlySpeed = 1;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioClip[] jumpSFX;

    [Header("Particles")]
    [SerializeField] private GameObject? speedyParticles;

    // private vars for me
        private int jumps;
        private List<string> statuses = new List<string>(); // process if character is sliding, jumping, etc.
        private Rigidbody2D rb;
        private bool slideCoroutineRunning; // for slide coroutine running
        private float hasteSpeed = 0; // this will be updated from entering a haste potion

    void Awake()
    {
        // get rigidbody
            rb = GetComponent<Rigidbody2D>();

        // set jump trigger function
            if (jumpTrigger != null)
            {
                jumpTrigger.EnterTrigger += OnJumpTriggerEnter;
                jumpTrigger.ExitTrigger += OnJumpTriggerExit;
            }

        // "raise" console error if entity has no rigidbody
            if (rb == null)
                Debug.Log("ERROR: "+this.name+" has no Rigidbody2D!");
    }

    void Update()
    {}



    // When called, it will process the input force 
    // and move the character in said direction
        public void Strafe(float inputForce)
        {  
            // stop movement changing if sliding
                if (HasStatus("sliding"))
                    return;

            float strafe_multiplier = GetSpeedMultiplier();

            // apply the speed multiplier to inputForce
                inputForce *= strafe_multiplier;

            // Calculate applied force based off current force. Then apply acceleration
                Vector2 addedForce = new Vector2((inputForce - rb.linearVelocity.x) * speedDampener, 0);

            // Add force to Rigidbody2D
                rb.AddForce(addedForce * rb.mass);
        }


    // When called, it will attempt to make the character jump
        public void Jump()
        {
            // NOTE: when fixed, add walljump

            // if there are jumps remaining -> attempt to jump
                if (jumps > 0)
                {
                    // get jumpVelocity (currently with a little check to make sure player cant just jump easy in air)
                        float yVel = jumpVelocity / (rb.linearVelocity.y < -1 ? 1.5f : 1);

                    // set velocity to add player jump
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, yVel);

                    // remove a jump
                        jumps--;

                    // handle statuses
                        if (jumps == maxJumps-1)
                            AddStatus("jumping");

                        if (!HasStatus("in_air"))
                            AddStatus("in_air");


                    // play jump SFX
                        SoundFXManager.instance.PlayRandomSoundClip(jumpSFX, transform, 1f);
                }
                
        }

    // refreshes character jump when called
        public void RefreshJump()
        {
            jumps = maxJumps;
        }


    // When called, the character will try to fly
        public void Fly(Vector2 inputDirection)
        {
            // Takes input dir and multiplies it by the x & y fly speed
                Vector2 inputForce = inputDirection.normalized * new Vector2(horizontalFlySpeed, verticalFlySpeed);

            // add input force
                rb.AddForce(inputForce);

        }


    // puts character in crouched position & checks for sliding
        public void Crouch()
        {
            // NOTE: crouch currently sets character to 5/6 scale, set with magic numbers
            //       not really sure what I want to do with crouch so this is good for now. - C

            // Get scale
                Vector3 scale = transform.localScale;

            // update y scale
                scale.y = 5*scale.y / 6;
            
            // translate character to account for scale change
                transform.position -= new Vector3(0, 1/6 * scale.y, 0);

            AddStatus("crouching");

            // If moving into crouch -> start sliding and get sliding boost
                if(Mathf.Abs(rb.linearVelocity.x) > SlideSpeedThreshold && !slideCoroutineRunning) // ADD slide delay
                {
                    Slide();
                }

            // Update scale
                transform.localScale = scale;
        }

    // un-crouches the character
        public void UnCrouch()
        {
            // Get scale
                Vector3 scale = transform.localScale;

            // move player up to account for scale change
                transform.position += new Vector3(0, 1/6f * scale.y, 0);

            // scale player back up to normal
                scale.y = scale.y * 6/5;

            RemoveStatus("sliding");
            RemoveStatus("crouching");

            // Update scale
                transform.localScale = scale;

            // turn off slide particles
                if (!HasStatus("speed")) speedyParticles.SetActive(false);
        }

    // Make player slide along the floor
        public void Slide()
        {
            // begin slide timer
                StartCoroutine(StartSlide());
            

            // get player scale
                float xScale = transform.localScale.x;

            // get new x velocity
                float xVel = xScale * slideVelocity;
                
            // update player velocity
                rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y);
        }



    // returns current speed multiplier on character
        public float GetSpeedMultiplier()
    {
        // auto set multiplier to regular speed
            float multiplier = strafeSpeed;

        // if crouching -> set mult to crouch speed
            if (HasStatus("crouching"))
                multiplier = crouchSpeed;

        // if in air -> set mult to air speed
            else if (HasStatus("in_air") || HasStatus("jumping"))
                multiplier = airSpeed;

        
        if (HasStatus("speed"))
            multiplier += hasteSpeed; // NOTE: figure out some way to update speed according to potion

        return multiplier;

    }



    // NOTE TO SELF ----------------------------------------------------------------------
    // We should probably just have the haste potion call this method instead
    // so on trigger enter -> add speed to statuses, wait time, remove speed status
    // we may want to do on trigger enter -> add speed, then on trigger exit -> StartCoroutine(SpeedUp())?
        public IEnumerator SpeedUp(float linger)
        {
            AddStatus("speed");

            speedyParticles.SetActive(true);
            yield return new WaitForSeconds(linger);
            if (!HasStatus("sliding")) speedyParticles.SetActive(false);

            RemoveStatus("speed");
        }

    // Starts sliding timer when character starts sliding
        public IEnumerator StartSlide()
        {
            // disable player from sliding for period of time
                slideCoroutineRunning = true;
            // show slide particles
                speedyParticles.SetActive(true);            
            // add friction
                rb.linearDamping = .3f;
            AddStatus("sliding");


            yield return new WaitForSeconds(slideTime);


            RemoveStatus("sliding");
            // hide slide particles
                if (!HasStatus("speed")) speedyParticles.SetActive(false);
            // remove friction
                rb.linearDamping = 0;
            // allow player slide
                slideCoroutineRunning = false;
        }
    

    // add / remove for statuses list
        public void AddStatus(string newStatus)
        {
            if (!HasStatus(newStatus))
                statuses.Add(newStatus);
        }
        public void RemoveStatus(string remStatus)
        {
            statuses.Remove(remStatus);
        }

    // check for status
        public bool HasStatus(string status)
        {
            return statuses.Contains(status);
        }

    // refreshes jump when touching ground
    public void OnJumpTriggerEnter(Collider2D other)
    {
        RefreshJump();

        // remove in air atatuses
            RemoveStatus("in_air");
            RemoveStatus("jumping");
    }

    public void OnJumpTriggerExit(Collider2D other)
    {
        // I guess the floor disappears before entities on closing game
        // this is here to make sure the coroutine doesnt occur while closing game
            if (!gameObject.activeSelf)
                return;

        if (other.gameObject.tag == "Ground")
            StartCoroutine(CoyoteTime());
    }

    public IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);

        if (jumps == maxJumps && Mathf.Abs(rb.linearVelocity.y) > 0.1f)
        {
            Debug.Log(jumps + " " + rb.linearVelocity.y);

            jumps--;
        }
    }
}
