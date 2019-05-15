using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertWhenInRange : MonoBehaviour
{

    private WalkTowardsPlayer target;
    
    void Start()
    {
        target = GetComponentInParent<WalkTowardsPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target.shouldChasePlayer = true;

        } 
        
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.gameObject.tag == "Player")
         {
            target.shouldChasePlayer = false;

         }

    }

}
