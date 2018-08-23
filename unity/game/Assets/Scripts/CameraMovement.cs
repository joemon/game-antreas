using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject rocket;

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        Vector3 rocketPosition = new Vector3 (rocket.transform.position.x, 0 , 0);
        offset = transform.position - rocketPosition;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        Vector3 rocketPosition = new Vector3(rocket.transform.position.x - 20f, 0, 0);
        transform.position = rocketPosition + offset;
	}
}
