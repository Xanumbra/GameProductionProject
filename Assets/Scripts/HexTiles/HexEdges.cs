using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdges : MonoBehaviour
{
    public HexVertices[] connectedVertices = new HexVertices[2];

    public bool hasRoad = false;
    public bool localPlayerOwnsRoad = false;
    public int ownerIndex = -1;

    public bool IsRoadValid()
    {
        foreach (var vertex in connectedVertices)
        {
            if (vertex.hasSettlement && vertex.localPlayerOwnsSettlement)
            {
                return true;
            }

            if (vertex.HasNeighborRoad(this))
            {
                return true;
            }
        }

        Debug.Log("Cannot build Road here - Must be next to Settlement or other Road");
        return false;
    }

    public bool HasNeighborSettlement(HexVertices caller)
    {
        foreach (var vertex in connectedVertices)
        {
            if (vertex != caller && vertex.hasSettlement)
            {
                return true;
            }
        }

        return false;
    }
}
