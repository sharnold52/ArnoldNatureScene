using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour
{
    // terrain that has the flow field
    public Terrain terrain;

    // field representing flow vectors
    public Vector3[,] field;

    // flow vectors' info
    Vector3 direction = Vector3.zero;

    // flow field adjustment values
    float a = 3f;
    float b = -3f;
    float c = 3f;
    float d = -3f;

	// Use this for initialization
	void Start ()
    {
        // set width depth and height of field
        field = new Vector3[(int)terrain.terrainData.size.x, (int)terrain.terrainData.size.z];
        
        for (int i = 0; i < (int)terrain.terrainData.size.x; i++)
        {
            for (int j = 0; j < (int)terrain.terrainData.size.z; j++)
            {
                direction.x = a*(i+1 - 100) + b;
                direction.z = c*(i+1 - 100) + d*(j+1 - 100);

                field[i, j] = direction;
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // get the direction of the flow vector
    public Vector3 GetFlowDirection(Vector3 location)
    {
        Vector3 flowDirection = Vector3.zero;

        // check if in bounds
        if(location.x < field.GetLength(0) && location.z < field.GetLength(1))
        {
            int x = Mathf.FloorToInt(location.x);
            int y = Mathf.FloorToInt(location.y);
            if(x > 0 && y > 0)
            {
                flowDirection = field[x, y];
            }
        }
        // out of bounds
        else
        {
            
        }

        return flowDirection;
    }
}
