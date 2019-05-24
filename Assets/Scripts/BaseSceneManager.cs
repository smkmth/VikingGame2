using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);
    }
    void DoTests()
    {

        if (LayerMask.NameToLayer("Path") == (LayerMask.NameToLayer("Path") | (1 << 11)))
        {
            Debug.LogError("No Path Layer! we need a Path layer, and every path to be tagged Path");
        }
        if (LayerMask.NameToLayer("Blocking") == (LayerMask.NameToLayer("Blocking") | (1 << 12)))
        {
            Debug.LogError("No Blocking layer, we need a Blocking layer, and every obstical to be tagged Blocking");
        }
        if (LayerMask.NameToLayer("Ground") == (LayerMask.NameToLayer("Ground") | (1 << 12)))
        {
            Debug.LogError("No Ground layer, we need a Ground layer, and every obstical to be tagged Ground");
        }
        if (!GameObject.Find("Pathfinding"))
        {
            Debug.LogError("No Pathfinding gameobject found; do you have an object called 'Pathfinding' in the scene with a valid" +
            "Pathfinding script attached to it?");
        }
        if (!GameObject.Find("Pathfinding").GetComponent<Pathfinding>())
        {
            Debug.LogError("No Pathfinding script found; You need to attach a 'Pathfinding' script to the gameobject called Pathfinding");
        }
        //grid tests

        //GRID TESTING
        if (!GameObject.Find("Pathfinder").GetComponent<WorldGrid>())
        {
            Debug.LogError("No Grid script found; You need to attach a 'Grid' script to the gameobject called Pathfinder if you " +
                "wish to generate a grid of nodes");
        }
        else
        {

            if (GameObject.Find("Pathfinder").GetComponent<WorldGrid>().gridWorldSize.x <= 0)
            {
                Debug.LogWarning("The gridsize x is set to 0 or less, so no nodes will be generated!");
            }
            if (GameObject.Find("Pathfinder").GetComponent<WorldGrid>().gridWorldSize.y <= 0)
            {
                Debug.LogWarning("The gridsize y is set to 0 or less, so no nodes will be generated!");
            }
            if (GameObject.Find("Pathfinder").GetComponent<WorldGrid>().nodeRadius <= 0)
            {
                Debug.LogWarning("The node rad is set to 0 or less, so no nodes will be generated!");
            }
            if (GameObject.Find("Pathfinder").GetComponent<WorldGrid>().walkableRegions.Length <= 0)
            {
                Debug.LogWarning("There are no walkable regions set!");
            }
            if (GameObject.Find("Pathfinder").GetComponent<WorldGrid>().obstacleProximityPenalty <= 0)
            {
                Debug.LogWarning("There is no penalty to obsticals!");
            }


        }
        //AGENT TESTING
        if (!GameObject.FindObjectOfType<PathMover>())
        {
            Debug.LogWarning("No Agents in the scene, you should create a gameobject with an agent script if you want to have a thing" +
                " follow a path!");
        }
        else
        {
            PathMover[] agents = GameObject.FindObjectsOfType<PathMover>();
            foreach (PathMover agent in agents)
            {
                if (!agent.gameObject.GetComponent<PathMover>())
                {
                    Debug.LogError("Agent " + agent.name + " has no PathMover!");
                }
                else
                {
                    if (agent.gameObject.GetComponent<PathMover>().speed <= 0)
                    {
                        Debug.LogWarning("Agent " + agent.name + " has no movespeed, and wont be able to move!");
                    }
                    if (agent.gameObject.GetComponent<PathMover>().stoppingDst <= 0)
                    {
                        Debug.LogWarning("Agent " + agent.name + " has no stopping distance, and will attempt to hit the exact point" +
                            " they are told, which can lead to unexpected behavior!");
                    }
                    if (agent.gameObject.GetComponent<PathMover>().turnSpeed <= 0)
                    {
                        Debug.LogWarning("Agent " + agent.name + " has no turn speed, and wont be able to turn properly!");
                    }
                    if (agent.gameObject.GetComponent<PathMover>().turnDst <= 0)
                    {
                        Debug.LogWarning("Agent " + agent.name + " has no turn distance, and wont be able to turn properly!");
                    }
                    if (agent.gameObject.GetComponent<PathMover>().repathRate <= 0)
                    {
                        Debug.LogWarning("Agent " + agent.name + " has no repath rate, and so wont update his path!");
                    }

                }
            }

            //stats testing

     
            {
                CameraControl playercam = GameObject.FindObjectOfType<CameraControl>();
                if (!playercam)
                {
                    Debug.LogWarning("Player has no camera control script, how are you controlling the camera?!");


                }
               
            }

           
        }
    }


}
