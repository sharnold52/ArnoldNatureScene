using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFollower : Vehicle
{
    // ultimate force for calculating steering forces
    public Vector3 ultimateForce;

    // reference to managers and flow field
    AgentManager manager;
    FlowField field;

    // max force
    public float maxForce = 1f;

    // y value for calculating height of terrain
    float y;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        manager = FindObjectOfType<AgentManager>();
        field = FindObjectOfType<FlowField>();
    }

    public override void CalcSteeringForces()
    {
        // calculate terrain height
        float terrainHeight = manager.terrain.SampleHeight(new Vector3(position.x, 0, position.z));

        // set y depending on where they are
        if(terrainHeight < 10f)
        {
            y = 14f;
        }
        else
        {
            y = 4 + manager.terrain.SampleHeight(new Vector3(position.x, 0, position.z));
        }

        // set y position and y direction
        position.y = y + gameObject.transform.lossyScale.y;

        // stay in bounds and avoid game objects
        Wrap();
        for (int i = 0; i < manager.allObstacles.Count; i++)
        {
            ultimateForce += AvoidObstacle(manager.allObstacles[i], 5f);
        }

        // Move along flow field
        ultimateForce += FollowField(field, transform.position);

        // separate from other flow...ers
        ultimateForce += Separate(manager.allFlowFollowers) * 1.5f;

        // clamp the ultimate force to Maximum force
        ultimateForce.y = 0;
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Apply the ultimate force to the Human's acceleration
        ApplyForce(ultimateForce);

        // ultimate force that's zeroed out
        ultimateForce = Vector3.zero;
    }
}