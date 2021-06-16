using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCameraRotator : MonoBehaviour
{
    void Update()
    {       
        Rotate(this.gameObject, Camera.main);
    }

    void Rotate(GameObject toRotate, Camera camera)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, camera.transform.rotation, 200 * Time.deltaTime);
    }
}
