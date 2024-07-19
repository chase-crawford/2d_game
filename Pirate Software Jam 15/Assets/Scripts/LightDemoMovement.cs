using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDemoMovement : MonoBehaviour
{

    public float moveSpeed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, 0);
        transform.position += new Vector3(0, Input.GetAxis("Vertical") * moveSpeed, 0);
    }
}
