using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyBehaviour
{
    Search,
    DirectEngagement,
    Flank
 
}

public class HostileAI : MonoBehaviour
{

    private PathMover pathMover;
    public GameObject target;
    private Combat combat;
    private bool inEngageRange;
    public float attackDistance = 1.0f;
    public float engageDistance = 5.0f;
    public bool freeMove;
    public enemyBehaviour thisBehaviour;
    public float behaviourChangeTime;
    private float behaviourTimer ;

    // Use this for initialization
    void Start () {
        //target = GameObject.Find("Player");
        pathMover = GetComponent<PathMover>();
        combat = GetComponent<Combat>();
        pathMover.target= target.transform;
        inEngageRange = false;
       

        freeMove = true;
    }

    void DecideBehaviour()
    {
        if (Random.Range(1,30) < 5)
        {
            if (thisBehaviour == enemyBehaviour.DirectEngagement)
            {
                thisBehaviour = enemyBehaviour.Flank;
            }
            else
            {
                thisBehaviour = enemyBehaviour.DirectEngagement;

            }

        }
    }

    // Update is called once per frame
    void Update () {
        if (freeMove)
        {

            float dist = Mathf.Abs(Vector3.Distance(target.transform.position, transform.position));
            if (inEngageRange)
            {
                switch (thisBehaviour)
                {
                    case enemyBehaviour.Search:

                        break;
                    case enemyBehaviour.DirectEngagement:
                        transform.LookAt(target.transform);

                        if (!combat.isAttacking)
                        {
                            if (dist > attackDistance)
                            {
                                DecideBehaviour();
                                if (!combat.isAttacking)
                                {
                                    transform.position += transform.forward * Time.deltaTime * combat.currentMovementSpeed;
                                }
                            }
                            else
                            {
                                if (!combat.isAttacking)
                                {
                                    combat.Attack();
                                }
                            }
                        }
                        break;
                    case enemyBehaviour.Flank:
                        if (!combat.isAttacking)
                        {
                            if (dist > attackDistance)
                            {
                                transform.LookAt(target.transform);
                               
                                transform.position += transform.right * Time.deltaTime * combat.currentMovementSpeed;
                                if (Vector3.Dot(target.transform.position, transform.forward) > 0)
                                {

                                }
                                else
                                {
                                    DecideBehaviour();
                                }

                            }
                            else
                            {
                                transform.position += -transform.forward * Time.deltaTime * combat.currentMovementSpeed;
                               
                            }

                        }

                        break;

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
                if (!combat.isAttacking)
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
    
}
