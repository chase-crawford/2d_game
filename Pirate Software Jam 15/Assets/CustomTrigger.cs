using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public event Action<Collider2D> EnterTrigger;
    public event Action<Collider2D> ExitTrigger;
    public event Action<Collider2D> StayTrigger;
    
    public event Action<Collision2D> EnterCollider;
    public event Action<Collision2D> ExitCollider;
    public event Action<Collision2D> StayCollider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnterTrigger?.Invoke(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ExitTrigger?.Invoke(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        StayTrigger?.Invoke(other);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Enter");
        EnterCollider?.Invoke(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Exit");
        ExitCollider?.Invoke(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Stay");
        StayCollider?.Invoke(collision);
    }
}
