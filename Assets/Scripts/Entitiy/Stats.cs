using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stats : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    private Rigidbody rb;
    public Vector3 startPos;

    public effectType currentEffect = effectType.nothing;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Health = MaxHealth;
        startPos = transform.position;

    }

    private void Update()
    {
        switch (currentEffect)
        {
            case effectType.bleeding:
                break;
            case effectType.instaKill:
                break;
            case effectType.poison:
                break;
            case effectType.slowed:
                break;

        }
    }


    public void DoDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
     
    }


    public void DoDamage(int amount,  float force)
    {
        Health -= amount;
        rb.AddForce(-transform.forward * force, ForceMode.Impulse);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }

    }




}
