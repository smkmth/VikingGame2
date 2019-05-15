using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public Transform target;
    public float Damping;

    [HideInInspector]
    public bool IsLockedOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        IsLockedOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLockedOn)
        {
            // transform.LookAt(target);

            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);

        }

    }
}
