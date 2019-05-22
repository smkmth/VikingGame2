using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour {

    public Weapon weaponData;
    public string target;

    private void OnTriggerEnter(Collider collision)
    {



        if (collision.transform.gameObject.tag == target)
        {

            collision.transform.gameObject.GetComponent<Stats>().DoDamage(weaponData.attackDamage, 3.0f);
            Debug.Log("Daamge");


        }


    }
}
