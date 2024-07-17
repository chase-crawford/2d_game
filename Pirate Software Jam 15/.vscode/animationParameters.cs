using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationParameters : MonoBehaviour
{
    public Animator animator;
    public CharacterController2D controller;
    public Movement inputs;
    private Rigidbody2D m_Rigidbody2D;
    public bool grounded = true;
    public bool crounching = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int moving = (int)m_Rigidbody2D.velocity.x;
        if(controller.m_Grounded == false)
        {
            animator.SetBool("inAir",true);
            grounded = false;
        }
        else    
        {
            animator.SetBool("inAir",false);
            grounded = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("crounching", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))  
        {
            animator.SetBool("crounching", false);
        }

        animator.SetInteger("isMoving",moving);
    }
}
