using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // calculated center of flock
    public Vector3 centerFlock = Vector3.zero;
    public Vector3 centerWanderers = Vector3.zero;

    // debug lines
    public Material greenLine;

    // average direction of flock
    Vector3 direction;

    // manager reference
    AgentManager manager;

	// Use this for initialization
	void Start ()
    {
        manager = FindObjectOfType<AgentManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // calculate direction of flockers
        direction = CalcDirection(manager.allFlockers);

        centerFlock = CalcCenter(manager.allFlockers);
        centerWanderers = CalcCenter(manager.allWanderers);

    }

    // calculate the center
    Vector3 CalcCenter(List<GameObject> flockers)
    {
        // sum up positions
        Vector3 sum = Vector3.zero;

        for(int i = 0; i < flockers.Count; i++)
        {
            sum += flockers[i].transform.position;
        }

        // average out position
        sum = sum / flockers.Count;

        return sum;
    }

    Vector3 CalcDirection(List<GameObject> flockers)
    {
        Vector3 sum = Vector3.zero;
        Vector3 aveDirection = Vector3.zero;
        float count = 0;

        for (int i = 0; i < flockers.Count; i++)
        {
            if (flockers[i] != null)
            {
                sum += flockers[i].GetComponent<Vehicle>().velocity;
                count += 1f;
            }
        }
        
        if (count > 0)
        {
            sum = sum / count;
            sum = sum.normalized;
            aveDirection = sum;
        }

        return aveDirection;
    }

    // draw debug lines
    private void OnRenderObject()
    {
        if (manager.debug)
        {
            FlowField fieldThing = GameObject.FindObjectOfType<FlowField>();
            PathFollowers follower = GameObject.FindObjectOfType<PathFollowers>();

            // set the material
            greenLine.SetPass(0);

            // draws one line
            GL.Begin(GL.LINES);
            GL.Vertex(centerFlock);
            GL.Vertex(centerFlock + (direction*10));
            GL.End();

            // draws flow field
            for (int i = 0; i < fieldThing.field.GetLength(0); i+=10)
            {
                for (int j = 0; j < fieldThing.field.GetLength(1); j+=10)
                {
                    Vector3 fieldVector = fieldThing.field[i, j].normalized * 5;
                    Vector3 start = new Vector3(i, 100, j);
                    Vector3 end = new Vector3(i + fieldVector.x, 100, j + fieldVector.z);
                    GL.Begin(GL.LINES);
                    GL.Vertex(start);
                    GL.Vertex(end);
                    GL.End();
                }
            }

            // draws lines for path
            for (int i = 0; i < follower.path.Length; i++)
            {
                if(i < follower.path.Length - 1)
                {
                    GL.Begin(GL.LINES);
                    GL.Vertex(follower.path[i].transform.position);
                    GL.Vertex(follower.path[i + 1].transform.position);
                    GL.End();
                }
                else
                {
                    GL.Begin(GL.LINES);
                    GL.Vertex(follower.path[i].transform.position);
                    GL.Vertex(follower.path[0].transform.position);
                    GL.End();
                }
            }
        }
    }
}
