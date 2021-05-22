using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertices : MonoBehaviour
{
    public List<HexEdges> connectedEdges;
    public bool isHarbor = false;
    public int harborType = 0;
    public Enums.Resources harborResource;
}
