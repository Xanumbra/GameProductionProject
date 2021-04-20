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

    void UpdateOwner(Player oldVal, Player newVal)
    {
        GetComponent<MeshRenderer>().material.color = newVal.playerColor;
    }

    void UpdateParent(Transform oldVal, Transform newVal)
    {
        transform.SetParent(newVal);
    }
}
