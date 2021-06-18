using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Range(0, 100)]
    public float rotSpeedYmin = 12;
    [Range(0, 100)]
    public float rotSpeedYmax = 12;

    [Range(0, 100)]
    public float rotSpeedXmin = 0;
    [Range(0, 100)]
    public float rotSpeedXmax = 0;

    [Range(0, 100)]
    public float rotSpeedZmin = 0;
    [Range(0, 100)]
    public float rotSpeedZmax = 0;

    public float rotSpeedY;
    public float rotSpeedX;
    public float rotSpeedZ;

    private void Start()
    {
        //Random.InitState(System.Environment.TickCount);

        rotSpeedY = Random.Range(rotSpeedYmin, rotSpeedYmax);
        rotSpeedX = Random.Range(rotSpeedXmin, rotSpeedXmax);
        rotSpeedZ = Random.Range(rotSpeedZmin, rotSpeedZmax);

        if (Random.Range(0, 100) > 50) rotSpeedY *= -1;
        if (Random.Range(0, 100) > 50) rotSpeedX *= -1;
        if (Random.Range(0, 100) > 50) rotSpeedZ *= -1;
    }

    private void Update()
    {
        transform.Rotate(rotSpeedX * Time.deltaTime, rotSpeedY * Time.deltaTime, rotSpeedZ * Time.deltaTime);
    }
}
