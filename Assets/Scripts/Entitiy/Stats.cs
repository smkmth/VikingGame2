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

    public float staggeredRecoveryTime;
    private float staggerTimer;

    private Actor actor;
    public effectType currentEffect = effectType.nothing;



    private void Start()
    {


        actor = GetComponent<Actor>();
        rb = GetComponent<Rigidbody>();
        Health = MaxHealth;
        startPos = transform.position;

    }
    private void Stagger(bool staggered)
    {
        if (staggered)
        {
            actor.FreeCombat = false;

        }
        else
        {
            actor.FreeCombat = true;
        }

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
            case effectType.staggered:
                if (staggerTimer >= staggeredRecoveryTime)
                {
                    Stagger(false);
                    currentEffect = effectType.nothing;
                    Debug.Log("not staggered");
                    staggerTimer = 0.0f;
                }
                else
                {
                    Stagger(true);

                    Debug.Log("staggered");
                    staggerTimer += Time.deltaTime;
                
                }
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

        currentEffect = effectType.staggered;
        staggerTimer = 0.0f;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

    }




}
