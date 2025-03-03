using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public GameObject bouncepadPrefab;

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag != "Player")
        {
            Break();
        }

    }

    void Break()
    {
        Destroy(gameObject);

        GameObject bouncepad = Instantiate(bouncepadPrefab, transform.position, Quaternion.identity);
    }
}
