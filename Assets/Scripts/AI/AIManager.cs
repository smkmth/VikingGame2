using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public HostileAI[] hostileAis;
    public Building buildings;
    public GameObject player;
	public void Start()
    {
        StartAttack();
    }
    public void StartAttack()
    {
        foreach(HostileAI ai in hostileAis)
        {

            ai.target = buildings.gameObject;
            ai.GetComponent<PathMover>().target = buildings.attackPoints[Random.Range(0, buildings.attackPoints.Length)].location;
        }
    }

}
