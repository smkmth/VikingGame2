using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathToPlayer : MonoBehaviour {

    private PathMover pathMover;
    private GameObject target;
    public float attackDist = 5.0f;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Player");
        pathMover = GetComponent<PathMover>();
        pathMover.target= target.transform;
    }

    // Update is called once per frame
    void Update () {

        if(Mathf.Abs(Vector3.Distance(target.transform.position, transform.position)) < attackDist)
        {
            DoAttack();
        }
        


    }

    void DoAttack()
    {
    }
}
