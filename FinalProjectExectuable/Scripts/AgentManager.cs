using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    // prefabs of flockers
    public GameObject flockerPrefab;
    public GameObject flowFollowerPrefab;
    public GameObject wandererPrefab;

    // number of flockers
    public int flockerCount;
    public int flowFollowerCount;
    public int wandererCount;

    // flocker lists
    public List<GameObject> allFlockers = new List<GameObject>();
    public List<GameObject> allFlowFollowers = new List<GameObject>();
    public List<GameObject> allWanderers = new List<GameObject>();

    // object list
    public List<GameObject> allObstacles = new List<GameObject>();

    // resistance area
    public GameObject resistanceArea;


    // random x and z positions, y is set to terrain height
    float x;
    float z;
    float y;

    // the terrain
    public Terrain terrain;
    public float buffer = 4f;
    public Vector3 minPos;
    public Vector3 maxPos;
    public Vector3 centerPos;

    // draw debug lines in game view
    public bool debug = false;

	// Use this for initialization
	void Start ()
    {
        // current game object
        GameObject current;

        // get all obstacles
        Obstacle[] theObstacles = (Obstacle[])FindObjectsOfType(typeof (Obstacle));
        for(int i = 0; i < theObstacles.Length; i++)
        {
            allObstacles.Add(theObstacles[i].gameObject);
        }

        // terrain bounds
        minPos = terrain.terrainData.bounds.min;
        maxPos = terrain.terrainData.bounds.max;

        // apply buffer
        minPos.x += buffer;
        minPos.z += buffer;
        maxPos.x -= buffer;
        maxPos.z -= buffer;

        // instantiate flockers
        for (int i = 0; i < flockerCount; i++)
        {
            if( i < flockerCount / 2)
            {
                // generate position
                x = 89;
                z = 44;
                y = terrain.SampleHeight(new Vector3(x, 0, z));

                // create and add flocker to list
                current = Instantiate(flockerPrefab, new Vector3(x, y + (flockerPrefab.transform.lossyScale.y / 10), z), Quaternion.identity);
                current.GetComponent<PathFollowers>().position = new Vector3(x, y + (flockerPrefab.transform.lossyScale.y / 10), z);
                current.GetComponent<PathFollowers>().radius = flockerPrefab.transform.lossyScale.x / 2;

                allFlockers.Add(current);
            }
            else
            {
                // generate position
                x = 42;
                z = 43;
                y = terrain.SampleHeight(new Vector3(x, 0, z));

                // create and add flocker to list
                current = Instantiate(flockerPrefab, new Vector3(x, y + (flockerPrefab.transform.lossyScale.y / 10), z), Quaternion.identity);
                current.GetComponent<PathFollowers>().position = new Vector3(x, y + (flockerPrefab.transform.lossyScale.y / 10), z);
                current.GetComponent<PathFollowers>().radius = flockerPrefab.transform.lossyScale.x / 2;

                allFlockers.Add(current);
            }
        }

        // instantiate flow followers
        for(int i = 0; i < flowFollowerCount; i++)
        {
            // randomly generate position
            x = Random.Range(minPos.x, maxPos.x);
            z = Random.Range(minPos.z, maxPos.z);
            y = terrain.SampleHeight(new Vector3(x, 0, z));

            // create and add flocker to list
            current = Instantiate(flowFollowerPrefab, new Vector3(x, y + (flowFollowerPrefab.transform.lossyScale.y / 10), z), Quaternion.identity);
            current.GetComponent<FlowFollower>().position = new Vector3(x, y + (flowFollowerPrefab.transform.lossyScale.y / 10), z);
            current.GetComponent<FlowFollower>().radius = flockerPrefab.transform.lossyScale.x / 2;

            allFlowFollowers.Add(current);
        }

        // instantiate wanderers
        for (int i = 0; i < wandererCount; i++)
        {
            // randomly generate position
            x = Random.Range(75, 125);
            z = Random.Range(75, 125);
            y = terrain.SampleHeight(new Vector3(x, 0, z));

            // create and add flocker to list
            current = Instantiate(wandererPrefab, new Vector3(x, y + (wandererPrefab.transform.lossyScale.y / 10), z), Quaternion.identity);
            current.GetComponent<Wanderer>().position = new Vector3(x, y + (wandererPrefab.transform.lossyScale.y / 10), z);
            current.GetComponent<Wanderer>().radius = wandererPrefab.transform.lossyScale.x / 2;

            allWanderers.Add(current);
        }

        // center to seek when out of bounds
        centerPos = terrain.terrainData.bounds.center;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // turn debug lines on and off
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(debug)
            {
                debug = false;
                resistanceArea.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                debug = true;
                resistanceArea.GetComponent<Renderer>().enabled = true;
            }
        }
	}
}
