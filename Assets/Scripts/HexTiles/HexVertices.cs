using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertices : MonoBehaviour
{
    public List<HexEdges> connectedEdges;

    public bool hasSettlement = false;
    public bool localPlayerOwnsSettlement = false;

    public bool IsSettlementValid()
    {
        foreach (var edge in connectedEdges)
        {
            if (edge.HasNeighborSettlement(this))
            {

                return false;
            }
        }

        if (GameManager.Instance.curGameState != Enums.GameState.preGame)
        {
            if (HasNeighborRoad())
            {
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
