using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Building : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateOwner))]
    public Player owner;

    [SyncVar(hook = nameof(UpdateParent))]
    public Transform parent;

    [SyncVar(hook = nameof(UpdateType))]
    public Enums.BuildingType type;

    //[Range(-100, 100)]
    //public float rotSpeed = 12;

    void UpdateOwner(Player oldVal, Player newVal)
    {
        owner = newVal;
    }

    void UpdateParent(Transform oldVal, Transform newVal)
    {
        transform.SetParent(newVal);
    }

    void UpdateType(Enums.BuildingType oldVal, Enums.BuildingType newVal)
    {
    }

    //private void Update()
    //{
    //    if (type == Enums.BuildingType.City || type == Enums.BuildingType.Settlement)
    //    {
    //        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    //    }
    //}
}
