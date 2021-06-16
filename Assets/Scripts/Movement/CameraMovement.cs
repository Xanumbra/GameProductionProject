using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    //private float moveSpeed = 0.1f;
    //private float scrollSpeed = 1f;
    //private float rotationSpeed = 1f;
    //public bool movable = true;

    //public void setMovable(bool value) {
    //    movable = value;
    //}

    //public bool getMovable() {
    //    return this.movable;
    //}
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update () {
    //    if (movable) {
    //        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
    //            transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    //        }

    //        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
    //            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
    //        }

    //        if (Input.GetKey(KeyCode.K)) {
    //            transform.Rotate(0,rotationSpeed,0,0);
    //        }

    //        if (Input.GetKey(KeyCode.J)) {
    //            transform.Rotate(0,-rotationSpeed,0,0);
    //        }
    //    }
    //} 

    //#### END OF OLD CAMERA CONTROLER ####
    //#### NEW ON IS BELOW ###

    float horizontalInput;
    float verticalInput;
    float mouseHorizontalInput;
    float mouseVerticalInput;
    float mouseScrollWheelInput;
    private float zoomTime = 0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    [Range(0.1f, 5.0f)]
    public float moveSpeed = 1.0f;

    [Range(0.1f, 5.0f)]
    public float rotationSpeed = 1.0f;

    [Range(1f, 10000f)]
    public float zoomSpeed = 1000f;

    public float maxZoomDepth = 20;
    public float maxZoomHeight = 80;


    public Collider boundingBox;

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseScrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(1))
        {
            mouseHorizontalInput = Input.GetAxis("Mouse X");
            mouseVerticalInput = Input.GetAxis("Mouse Y");

        }
        else
        {
            mouseHorizontalInput = 0;
            mouseVerticalInput = 0;
        }

    }

    void Move()
    {
        transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * verticalInput * moveSpeed;
        if (!boundingBox.bounds.Contains(transform.position))
            transform.position -= new Vector3(transform.forward.x, 0, transform.forward.z) * verticalInput * moveSpeed;

        transform.position += transform.right * horizontalInput * moveSpeed;
        if (!boundingBox.bounds.Contains(transform.position))
            transform.position -= transform.right * horizontalInput * moveSpeed;
    }
    void Rotate()
    {
        transform.eulerAngles += new Vector3(mouseVerticalInput * (-1), mouseHorizontalInput, 0) * rotationSpeed;
    }

    void Zoom()
    {
        bool zoomingIn = mouseScrollWheelInput > 0;
        if (mouseScrollWheelInput != 0) zoomTime = 0;
        zoomTime += Time.deltaTime;

        var translation = transform.position + (transform.forward * mouseScrollWheelInput * zoomSpeed);
        if ((transform.position.y < maxZoomDepth && !zoomingIn || transform.position.y > maxZoomHeight && zoomingIn)
            || (transform.position.y > maxZoomDepth && transform.position.y < maxZoomHeight))
        {
            transform.position = Vector3.Lerp(transform.position, translation, zoomTime / 4f);
        }
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        GetInput();
        Move();
        Rotate();
    }

    private void Update()
    {
        Zoom();
    }

    public void ResetCameraBtn()
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
    }
}

