using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowers : Vehicle
{
    // ultimate force for calculating steering forces
    public Vector3 ultimateForce;

    // reference to agent manager
    AgentManager manager;
    SceneManager sManager;

    // path nodes
    public GameObject[] path;
    GameObject currentTarget;
    int currentNode;

    // max force
    public float maxForce = 1f;

    // max speed possible
    float maxSpeedPossible;

    // y value for calculating height of terrain
    float y;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        
        // populate path array
        for(int i = 0; i < path.Length; i++)
        {
            string nameOfNode = "wp" + i.ToString();
            path[i] = GameObject.Find(nameOfNode);
        }

        // keep track of possible max speed (For areas of resistance)
        maxSpeedPossible = maxSpeed;


        // set target
        currentTarget = path[0];
        currentNode = 0;

        manager = FindObjectOfType<AgentManager>();
        sManager = FindObjectOfType<SceneManager>();
    }

    public override void CalcSteeringForces()
    {
        // calculate y
        y = 3 + manager.terrain.SampleHeight(new Vector3(position.x, 0, position.z));

        // set y position and y direction
        position.y = y + gameObject.transform.lossyScale.y;

        // seek sphere
        if (currentTarget != null)
        {
            CheckPath(currentTarget.transform.position);
            ultimateForce += Seek(currentTarget.transform.position);
        }
        
        // stay in bounds
        ultimateForce += CheckBounds();

        // separate from other humans
        ultimateForce += Separate(manager.allFlockers) * 1.5f;
        ultimateForce += Seek(sManager.centerFlock) * 0.25f;
        ultimateForce += Align(manager.allFlockers);

        // clamp the ultimate force to Maximum force
        ultimateForce.y = 0;
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Apply the ultimate force to the Human's acceleration
        ApplyForce(ultimateForce);

        // ultimate force that's zeroed out
        ultimateForce = Vector3.zero;
    }

    void CheckPath(Vector3 target)
    {
        // check if flock is close to target
        if(Vector3.Distance(target, transform.position) < 6)
        {
            // increment through array
            if(currentNode < path.Length - 1)
            {
                currentNode++;
            }
            else
            {
                currentNode = 0;
            }

            // check for area of resistance
            if(path[currentNode].name == "wp17")
            {
                maxSpeed = 2f;
            }
            else
            {
                maxSpeed = maxSpeedPossible;
            }

            // set target
            currentTarget = path[currentNode];
        }
    }
}