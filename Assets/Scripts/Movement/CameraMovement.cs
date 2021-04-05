using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float moveSpeed = 0.1f;
    private float scrollSpeed = 1f;
    private float rotationSpeed = 1f;
    public bool movable = true;

    public void setMovable(bool value) {
        movable = value;
    }

    public bool getMovable() {
        return this.movable;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (movable) {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0) {
                transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
            }

            if (Input.GetKey(KeyCode.K)) {
                transform.Rotate(0,rotationSpeed,0,0);
            }

            if (Input.GetKey(KeyCode.J)) {
                transform.Rotate(0,-rotationSpeed,0,0);
            }
        }
    }
}

