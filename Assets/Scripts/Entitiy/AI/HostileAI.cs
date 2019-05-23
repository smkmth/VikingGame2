using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAI : Actor
{

    private PathMover pathMover;
    private GameObject target;
    private Animator animator;
    public WeaponHit weapon;
    public float attackTime;
    public float anticipationTime;
    public float engageDistance = 5.0f;
    private bool attacking;
    private bool inEngageRange;
    public float attackDistance =1.0f;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Player");
        pathMover = GetComponent<PathMover>();
        animator = GetComponent<Animator>();
        pathMover.target= target.transform;
        inEngageRange = false;
        FreeCombat = true;
        FreeLook = true;
        FreeMove = true;
    }

    // Update is called once per frame
    void Update () {
        if (FreeMove)
        {

            float dist = Mathf.Abs(Vector3.Distance(target.transform.position, transform.position));
            if (inEngageRange)
            {

                transform.LookAt(target.transform);
                if (!attacking)
                {
                    if (dist > attackDistance)
                    {
                        if(!attacking)
                        {
                            transform.position += transform.forward * Time.deltaTime * 2.0f;

                        }
                    }
                    else
                    {
                        if (FreeCombat)
                        {
                            if (!attacking)
                            {
                            

                                StopCoroutine(DoAttack());
                                StartCoroutine(DoAttack());
                            }

                        }

                    }
                } 

            }

            if (dist < engageDistance)
            {
                inEngageRange = true;
                pathMover.followingTarget = false;
         
            }
            else
            {
                inEngageRange = false;
                if (!attacking)
                {
                    pathMover.followingTarget = true;
                }
            }
        }
        else
        {
            pathMover.followingTarget = false;
        }
     

    }

    IEnumerator DoAttack()
    {
        attacking = true;
        transform.LookAt(target.transform);
        animator.SetTrigger("ReadyAttack");
        yield return new WaitForSeconds(anticipationTime);
        animator.SetTrigger("Attack");
        weapon.doDamage = true;
        yield return new WaitForSeconds(attackTime);
        weapon.doDamage = false;

        attacking = false;
    }
}
