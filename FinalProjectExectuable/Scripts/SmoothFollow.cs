using UnityEngine;
using System.Collections;
// This camera smoothes out rotation around the y-axis and height.
// Horizontal Distance to the target is always fixed.
// For every one of those smoothed values, calculate the wanted value and the current value.
// Smooth it using theLerp function and apply the smoothed values to the transform's position.
public class SmoothFollow: MonoBehaviour 
{
    public Transform target;
    public float distance = 20.0f;
    public float height = 10.50f;
    public float heightDamping = 2.0f;
    public float positionDamping =2.0f;
    public float rotationDamping = 2.0f;

    // set camera to follow a flow follower
    public bool flowFollower = false;

    // Update is called once per frame
    void LateUpdate ()
    {
        // get target
        if(target == null && !flowFollower)
        {
            target = FindObjectOfType<PathFollowers>().transform;
        }
        else if(target == null)
        {
            target = FindObjectOfType < FlowFollower>().transform;
        }

        // Early exit if there’s no target
        if (!target)return;

        float wantedHeight = target.position.y + height;
        float currentHeight = transform.position.y;

        // look at target
        gameObject.transform.LookAt(target);

        // Damp the height
        currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        
        // Set the position of the camera
        Vector3 wantedPosition = target.position -target.forward * distance;
        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * positionDamping);

        // Adjust the height of the camera
        transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);

        // Set the forward to rotate with time
        transform.forward = Vector3.Lerp (transform.forward, target.forward, Time.deltaTime * rotationDamping);
    }
}