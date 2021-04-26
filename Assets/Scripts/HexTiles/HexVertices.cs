using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertices : MonoBehaviour
{
    public List<HexEdges> connectedEdges;

    public bool hasSettlement = false;

    public bool IsBuildingValid()
    {
        foreach (var edge in connectedEdges)
        {
            if (edge.HasNeighborSettlement(this))
            {

                return false;
            }
        }

        return true;
    }
}
