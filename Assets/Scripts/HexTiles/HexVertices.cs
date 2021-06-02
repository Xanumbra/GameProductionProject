using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertices : MonoBehaviour
{
    public List<HexEdges> connectedEdges;

    public bool hasSettlement = false;
    public bool localPlayerOwnsSettlement = false;
    public int ownerIndex = -1;

    public bool IsSettlementValid()
    {
        foreach (var edge in connectedEdges)
        {
            if (edge.HasNeighborSettlement(this))
            {
                // Debug.Log("Cannot Build Settlement - Neighbor Settlement");
                InfoBoxManager.Instance.ErrorMessageOnClient("Cannot Build Settlement - Neighbor Settlement");
                return false;
            }
        }

        if (GameManager.Instance.curGameState != Enums.GameState.preGame)
        {
            if (HasNeighborRoad() == false)
            {
                Debug.Log("Cannot Build Settlement - Must be next to Road");
                InfoBoxManager.Instance.ErrorMessageOnClient("Cannot Build Settlement - Must be next to Road");
                return false;
            }
        }

        return true;
    }

    public bool HasNeighborRoad(HexEdges caller = null)
    {
        foreach (var edge in connectedEdges)
        {
            if (edge != caller && edge.hasRoad && edge.localPlayerOwnsRoad)
            {
                return true;
            }
        }

        return false;
    }
}
