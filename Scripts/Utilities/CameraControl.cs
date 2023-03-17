using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    public bool isControlEnabled;
    
    public float panSpeed = 100.0f;
    public float panSpeedBoost = 2;
    public float jumpSpeed = 370.0f; //Unused?
    public float panBorderThiccness = 1.0f;
    public float scrollDistance = 25.0f;

    public HexGrid hexGrid;
    public float cameraYMinPos = 30;
    public float cameraYMaxPos = 200;

    public float cameraXMinPos = -200;
    public float cameraXMaxPos = 200;

    public float cameraZMinPos = -200;
    public float cameraZMaxPos = 200;

    private float scrollSpeed = 20;
    private float scroll;
    private float cameraZoomPosition;

    private Vector3 pos;
    //private Quaternion rot;

    //Rotation
    public Texture2D cursorTexture;
    private Vector2 hotSpot = Vector2.zero;

    //0 INACTIVE
    //1 MOVING (unused)
    //2 ROTATING
    private int CAMERA_MODE = 0;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    public float rotationSpeed = (float)0.85;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        cameraZoomPosition = transform.position.y;
    }

    // Update is called once per frame
    // Handles Keyboard and Mouse Panning of Camera
    void Update()
    {
        if (isControlEnabled)
        {
            //rot = transform.rotation;
            HandleRotation();

            pos = transform.position;
            HandlePanning();

            HandleScrolling();

        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.R) && CAMERA_MODE != 2)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
            CAMERA_MODE = 2;
        }
        else if (!Input.GetKey(KeyCode.R) && CAMERA_MODE != 0)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset
            CAMERA_MODE = 0;
            lastMouseCoordinate = Vector3.zero;
        }

        if (CAMERA_MODE == 2)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
            if (mouseDelta.y > 0.5)
            {
                transform.Rotate(-rotationSpeed, 0, 0);
            }
            else if (mouseDelta.y < -0.5)
            {
                transform.Rotate(rotationSpeed, 0, 0);
            }

            lastMouseCoordinate = Input.mousePosition;
        }
    }

    private void HandlePanning()
    {
        //Pan Left
        if (Input.GetKey(KeyCode.A) && Input.mousePosition.x <= panBorderThiccness)
        {
            if (pos.x > cameraXMinPos) pos.x -= (panSpeedBoost * panSpeed) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThiccness)
        {
            if (pos.x > cameraXMinPos) pos.x -= panSpeed * Time.deltaTime;
        }

        //Pan Right
        if (Input.GetKey(KeyCode.D) && Input.mousePosition.x >= Screen.width - panBorderThiccness)
        {
            if (pos.x < cameraXMaxPos) pos.x += (panSpeedBoost * panSpeed) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThiccness)
        {
            if (pos.x < cameraXMaxPos) pos.x += panSpeed * Time.deltaTime;
        }

        //Pan Forward
        if (Input.GetKey(KeyCode.W) && Input.mousePosition.y >= Screen.height - panBorderThiccness)
        {
            if (pos.z < cameraZMaxPos) pos.z += (panSpeedBoost * panSpeed) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThiccness)
        {
            if (pos.z < cameraZMaxPos) pos.z += panSpeed * Time.deltaTime;
        }

        //Pan Backward
        if (Input.GetKey(KeyCode.S) && Input.mousePosition.y <= panBorderThiccness)
        {
            if (pos.z > cameraZMinPos) pos.z -= (panSpeedBoost * panSpeed) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThiccness)
        {
            if (pos.z > cameraZMinPos) pos.z -= panSpeed * Time.deltaTime;
        }

        transform.position = pos; // Submit Result
    }


    private void HandleScrolling()
    {
        //Check which direction the user is scrolling and store it
        scroll = Input.GetAxis("Mouse ScrollWheel");

        //Zoom In
        if (scroll > 0)
        {
            // cameraZoomPosition = transform.position.y;
            cameraZoomPosition -= scrollDistance;
        }
        else if (scroll < 0) //Zoom Out
        {
            // cameraZoomPosition = transform.position.y;
            cameraZoomPosition += scrollDistance;
        }

        //Restrict the camera y position
        cameraZoomPosition = Mathf.Clamp(cameraZoomPosition, cameraYMinPos, cameraYMaxPos);

        //Check if the current position matches the destination height
        if (Mathf.Abs(cameraZoomPosition - transform.position.y) > 1)
        {
            //Speed will deaccelerate as camera approaches target position
            scrollSpeed = (cameraZoomPosition - transform.position.y) * 5f;
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime, Space.World);
        }

    }

    public void Focus(Transform target, int offsetY, int offsetZ)
    {
        Vector3 targetRotation = transform.rotation.eulerAngles; //Get Vector 3 representation  
        targetRotation = new Vector3(targetRotation.x-15, targetRotation.y, targetRotation.z); //Adjust angle
        Quaternion targetRotationQ = Quaternion.Euler(targetRotation); //Convert back

       // IEnumerator coroutineRotate = RotateLerp(transform, targetRotationQ);
        IEnumerator coroutine = CameraJump(target.position, targetRotationQ, offsetY, offsetZ);

       // StopAllCoroutines();
        StartCoroutine(coroutine);
        //StartCoroutine(coroutineRotate);

    }
    public void UnFocus()
    {
       // IEnumerator coroutineRotate = RotateLerp(transform, originRotation);
        IEnumerator coroutine = CameraJump(originPosition, originRotation, 0, 0);
        StopAllCoroutines();
        StartCoroutine(coroutine);
        //StartCoroutine(coroutineRotate);
    }


    /* Quickly pans the camera in the direction of the given vector and sets it behind the target object
     * Disables user camera control while in effect
     */

    public IEnumerator CameraJump(Vector3 targetPosition, Quaternion targetRotationQ, int offsetY, int offsetZ)
    {
        targetPosition.y += offsetY;
        targetPosition.z -= offsetZ;
        bool shouldReEnable = isControlEnabled;
        isControlEnabled = false;
        float smoothTime = 0.3F;
        Vector3 velocity = Vector3.zero;

        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

        //Vector3.SqrMagnitude(targetPosition - transform.position) > 100.0f

        //While close enough, continue to move the camera
        while (Vector3.Distance(transform.position, targetPosition) > 3f)
        {

            //Debug.Log((Vector3.SqrMagnitude(targetPosition - transform.position)));
            //Debug.Log(Vector3.Distance(transform.position, targetPosition));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQ, Time.deltaTime * 3f);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpSpeed * Time.deltaTime);
            //  transform.Translate(Space.World);
            //Wait a frame and repeat
            yield return null;
        }

        //Reset camera target position to prevent auto scrolling after the jump
        cameraZoomPosition = transform.position.y;
        if(shouldReEnable) isControlEnabled = true;
    }

    /*
    IEnumerator RotateLerp(Transform target, Quaternion endRot)
    {
        float duration = 0.75f;
        var startRot = target.rotation; // current rotation
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            target.rotation = Quaternion.Slerp(startRot, endRot, timer / duration);
            yield return 0;
        }
    }
    */

    public void UpdateClampPositions(int cellCountX, int cellCountZ)
    {
       // cameraXMinPos = 20;
        cameraXMaxPos = (cellCountX - 0.5f) * (2f * HexMetrics.innerRadius) -20;
       // cameraZMinPos = -30;
        cameraZMaxPos = (cellCountZ - 1) * (1.5f * HexMetrics.outerRadius) - 50;
    }


}

