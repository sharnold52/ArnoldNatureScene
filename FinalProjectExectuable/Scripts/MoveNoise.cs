using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNoise : MonoBehaviour
{
    // reference to scene manager
    SceneManager sManager;


	// Use this for initialization
	void Start ()
    {
        // get scene manager
        sManager = FindObjectOfType<SceneManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // move audio source to current position of flock wanderers center
        gameObject.transform.position = sManager.centerWanderers;
	}
}
