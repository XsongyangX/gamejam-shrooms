using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // for the time to finish a camera pan
    const float transitionTime = 0.01f;
    // for the distance to cross a camera pan
    const float stepDistance = 3f;
    
    // move start time
    float moveStartTime;
    // current move time
    float moveCurrentTime;
    // end move time
    float moveEndTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    // Move the camera
    void move()
    {
        Vector3 direction = getDirection();
        if (direction == Vector3.zero) return; // bad key

        transform.position = transform.position + stepDistance * direction;

        //moveCurrentTime = Time.time;
        //moveStartTime = Time.time;
        //moveEndTime = Time.time + transitionTime;

        //// lerp
        //if (moveCurrentTime < moveEndTime)
        //{ 
        //    Transform transform = GetComponent<Transform>();

        //    float ratio = (moveCurrentTime - moveStartTime) / (moveEndTime - moveStartTime);
        //    Vector3 position = transform.position;
        //    Vector3 destination = position + stepDistance * direction;
        //    transform.position = Vector3.Lerp(position, destination, ratio);
        //}
    }

    // Get a direction
    private Vector3 getDirection()
    {
        // left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return new Vector3(1, 0, 0);
        // right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return new Vector3(-1, 0, 0);
        // up
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return new Vector3(0, -1, 0);
        // down
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return new Vector3(0, 1, 0);
        else
            return Vector3.zero;
    }
}
