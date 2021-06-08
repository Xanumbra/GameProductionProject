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
}
