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

    public float moveSpeed = 10;
    private float zoomLevel = 0;
    public Vector2 zoomLimits;
    private bool dragScreen = false;
    private Vector2 previousMousePos;

    public static CameraController main;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        MouseMovement();
        MouseZoom();
        previousMousePos = Input.mousePosition;
        UpdateAIControl();
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

    private void MouseMovement()
    {
        if (isControlledByAI) return;
        float distToScreenEdge = 0.1f * Screen.height;
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 delta = mousePosition - previousMousePos;

            transform.position -= new Vector3(delta.x, delta.y, 0) * 0.07f;// * Time.deltaTime;
        }
        else
        {
            if (mousePosition.x <= distToScreenEdge)
            {
                transform.position -= Vector3.right * Time.deltaTime * moveSpeed;
            }
            else if (mousePosition.x >= Screen.width - distToScreenEdge)
            {
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            }
            if (mousePosition.y <= distToScreenEdge)
            {
                transform.position -= Vector3.up * Time.deltaTime * moveSpeed;
            }
            else if (mousePosition.y >= Screen.height - distToScreenEdge && mousePosition.x < Screen.width * 0.25f || mousePosition.x > Screen.width * 0.75f)
            {
                transform.position += Vector3.up * Time.deltaTime * moveSpeed;
            }
        }
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 5, ListCreation.main.tileAmount / ListCreation.main.tilesPerRow * ListCreation.main.distanceX - 5);
        pos.y = Mathf.Clamp(pos.y, Mathf.Lerp(-10, -30, zoomLevel), ListCreation.main.tilesPerRow * ListCreation.main.distanceY - Mathf.Lerp(10, 20, zoomLevel));
        transform.position = pos;
    }
    private void MouseZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        zoomLevel -= scroll * Time.deltaTime * 3;
        zoomLevel = Mathf.Clamp01(zoomLevel);
        float zoom = Mathf.Lerp(zoomLimits.x, zoomLimits.y, zoomLevel);
        Vector3 pos = transform.position;
        pos.z = zoom;
        pos.y += scroll * 0.5f;
        transform.position = pos;
    }

    private bool isControlledByAI = false;
    private Vector3 targetPosition;
    public float moveSpeedAI = 5;
    public static void MoveTo(Vector3 position)
    {
        main.isControlledByAI = true;
        main.targetPosition = position + new Vector3(0, -20, -50);

    }
    private void UpdateAIControl()
    {
        if (!isControlledByAI) return;
        Vector3 toTarget = targetPosition - transform.position;
        if (toTarget.magnitude <= moveSpeedAI * Time.deltaTime * 2)
        {
            transform.position = targetPosition;
        } else
        {
            transform.position += toTarget.normalized * moveSpeedAI * Time.deltaTime;
        }
    }
    public static void ReleaseControl()
    {
        main.isControlledByAI = false;
    }
    public static void TakeControl()
    {
        main.isControlledByAI = true;
        main.targetPosition = main.transform.position;
    }
}
