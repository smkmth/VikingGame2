using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int MaxHealth;
    public int Health;

    public Vector3 startPos;

    public effectType currentEffect = effectType.nothing;

    

    private void Start()
    {
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

 

}
