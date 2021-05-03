using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;

    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        Vector3 sth = new Vector3(500000, 500000, 50000);
        transform.RotateAround(target.transform.position, sth, 20 * Time.deltaTime);
    }
}
