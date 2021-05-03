using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseDivide : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private int someInt = 0;
    bool clicked = false;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform cameraInit = Camera.main.transform;
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        screenPoint.z = 10.0f; //distance of the plane from the camera
        Debug.Log(screenPoint);
        //Debug.Log(Camera.main.ScreenToWorldPoint(screenPoint));
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Up");
            clicked = false;
        }
        if(clicked == true) {
            if(Input.mousePosition.x > Screen.width/2) {
                Vector3 sth = new Vector3(0, 1, 0);
                transform.RotateAround(target.transform.position, sth, 60 * Time.deltaTime);
            } else {
                Vector3 sth = new Vector3(0, 1, 0);
                transform.RotateAround(target.transform.position, sth, -60 * Time.deltaTime);
            }
        }
        //Debug.Log(Camera.main.transform.position);
    }
}
