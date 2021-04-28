using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdges : MonoBehaviour
{
    public HexVertices[] connectedVertices = new HexVertices[2];

    public bool hasRoad = false;
    public bool localPlayerOwnsRoad = false;

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
