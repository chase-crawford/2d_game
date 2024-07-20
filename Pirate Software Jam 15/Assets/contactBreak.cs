using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contactBreak : MonoBehaviour
{

    public LayerMask m_WhatIsGround;
    private BoxCollider2D flask;
    public float radius = 1;

    // Update is called once per frame
    void Update()
    {
        /*Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, m_WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
            {
				Destroy (gameObject);
            }
		}*/
    }

    void OnCollisionEnter2D(Collision2D flask)
    {
        if (flask.gameObject.tag == "Ground")
            Destroy (gameObject);
    }
}
