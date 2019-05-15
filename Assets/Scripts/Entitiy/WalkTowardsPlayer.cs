using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTowardsPlayer : MonoBehaviour
{

    public float moveSpeed;
    public bool shouldChasePlayer;
    public GameObject player;
    

    // Update is called once per frame
    void Update()
    {
        if (shouldChasePlayer)
        {
            float step = moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position,player.transform.position) >1.0f )
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position,step);
            }        
        }
    }
}
