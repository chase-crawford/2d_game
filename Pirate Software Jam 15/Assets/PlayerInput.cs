using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private MovementComponent move;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MovementComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector3 scale = transform.localScale;

        // get L/R input + U/D input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

        // Move L/R
            move.Strafe(horizontalInput);

        // update facing direction
            scale.x = horizontalInput > 0 ? Mathf.Abs(scale.x) : horizontalInput < 0 ? -Mathf.Abs(scale.x) : scale.x;
            transform.localScale = scale;


        if (Input.GetButtonDown("Crouch"))
        {
            move.Crouch();
        }

        if (Input.GetButtonUp("Crouch"))
        {
            move.UnCrouch();
        }

        if (Input.GetButtonDown("Jump"))
        {
            move.Jump();
        }
    }
}
