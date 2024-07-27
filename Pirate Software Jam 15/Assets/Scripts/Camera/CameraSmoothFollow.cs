using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public GameObject player;
    public bool YLock = false;
    public float speed= 1;
    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        if(YLock == false)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,  new Vector3(player.transform.position.x, player.transform.position.y, -6), Time.deltaTime * speed); 
        }
        else if(YLock == true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(player.transform.position.x, 0, -6), Time.deltaTime * speed);
        }

   
        
    }
}
