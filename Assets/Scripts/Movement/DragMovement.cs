using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMovement : MonoBehaviour
{
    public bool movable = true;
    public float dragSpeed = 1;
    private bool anotherBool;
    bool clicked = false;
    private Vector3 screenPoint;
    private Vector3 previousPoint;
    public GameObject objectToTurnAround;
    void Start()
    {
        
    }
    void Update()
    {
        screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        screenPoint.z = 10.0f; //distance of the plane from the camera
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            clicked = false;
        }
        if(clicked == true) {
            Vector3 turnVector = new Vector3(0, 1, 0);
            if (screenPoint.x - previousPoint.x < 0)
            {
                transform.RotateAround(objectToTurnAround.transform.position, turnVector, (0 - dragSpeed) * Math.Abs(screenPoint.x -
                    previousPoint.x) / 10);
            }
            else if ((screenPoint.x - previousPoint.x > 0))
            {
                transform.RotateAround(objectToTurnAround.transform.position, turnVector, dragSpeed * Math.Abs(screenPoint.x -
                    previousPoint.x) / 10);
            }
        }
        previousPoint = screenPoint;
    }
}