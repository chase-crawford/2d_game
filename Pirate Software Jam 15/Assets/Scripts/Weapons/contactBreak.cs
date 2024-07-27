using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contactBreak : MonoBehaviour
{

    public LayerMask m_WhatIsGround;
    public GameObject AOE;
    private BoxCollider2D flask;
    public float aoeHeight = 1;
    public float aoeWidth = 1;

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            GameObject aoe = Instantiate(AOE, transform.position, Quaternion.identity);
            Vector3 aoeScale = aoe.transform.localScale;
            aoeScale.x = aoeWidth;
            aoeScale.y = aoeHeight;
            aoe.transform.localScale = aoeScale;
            Destroy (gameObject);
        }
        
        
    }
}
