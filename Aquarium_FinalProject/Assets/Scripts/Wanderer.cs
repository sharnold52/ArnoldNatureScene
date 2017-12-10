using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Vehicle
{
    // ultimate force for calculating steering forces
    public Vector3 ultimateForce;

    // reference to managers
    AgentManager manager;
    SceneManager sManager;

    // max force
    public float maxForce = 1f;

    // y value for calculating height of terrain
    float y;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        manager = FindObjectOfType<AgentManager>();
        sManager = FindObjectOfType<SceneManager>();
    }

    public override void CalcSteeringForces()
    {
        // calculate y
        y = manager.terrain.SampleHeight(new Vector3(position.x, 0, position.z));

        // set y position and y direction
        position.y = y;

        // wander
        ultimateForce += Wander();

        // stay in bounds and avoid obstacles
        ultimateForce += CheckLand() * 2f;
        for(int i = 0; i < manager.allObstacles.Count; i++)
        {
            ultimateForce += AvoidObstacle(manager.allObstacles[i], 5f);
        }

        // separate from other wanderers and flock
        ultimateForce += Separate(manager.allWanderers) * 1.5f;
        ultimateForce += Align(manager.allWanderers) * 2.5f;
        ultimateForce += Seek(sManager.centerWanderers) * 0.5f;

        // clamp the ultimate force to Maximum force
        ultimateForce.y = 0;
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Apply the ultimate force to the Human's acceleration
        ApplyForce(ultimateForce);

        // ultimate force that's zeroed out
        ultimateForce = Vector3.zero;
    }
}