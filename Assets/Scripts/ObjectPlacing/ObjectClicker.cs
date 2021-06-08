using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectClicker : MonoBehaviour
{
    public HexGrid hexGrid;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("Object Clicker Enabled");
    }

    private void OnDisable()
    {
        Debug.Log("Object Clicker Disabled");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                var clickedObject = hit.transform.gameObject;
                PrintName(clickedObject);
                Enums.BuildingType type = Enums.BuildingType.None;
                if (clickedObject.name.Contains("Edge"))
                {
                    type = Enums.BuildingType.Road;
                    var hexEdge = clickedObject.GetComponent<HexEdges>();
                    if (hexEdge.IsRoadValid())
                    {
                        if ((GameManager.Instance.curGameState != Enums.GameState.preGame && Player.localPlayer.HasResources(type))
                            || GameManager.Instance.curGameState == Enums.GameState.preGame)
                        {
                            Player.localPlayer.PlaceBuilding(clickedObject, type, hexGrid.hexEdges.IndexOf(hexEdge));
                        }
                        else
                        {
                            ResourceError();
                        }
                    }
                }
                else if (clickedObject.name.Contains("Vertex"))
                {
                    type = Enums.BuildingType.Settlement;
                    var hexVertex = clickedObject.GetComponent<HexVertices>();
                    if (hexVertex.IsSettlementValid())
                    {
                        if ((GameManager.Instance.curGameState != Enums.GameState.preGame && Player.localPlayer.HasResources(type))
                            || GameManager.Instance.curGameState == Enums.GameState.preGame)
                        {
                            Player.localPlayer.PlaceBuilding(clickedObject, type, hexGrid.hexVertices.IndexOf(hexVertex));
                        }
                        else
                        {
                            ResourceError();
                        }
                    }
                }
                else if (clickedObject.name.Contains("Settlement"))
                {
                    Debug.Log("Settlement Clicked");

                    //if ((GameManager.Instance.curGameState != Enums.GameState.preGame && Player.localPlayer.HasResources(Enums.BuildingType.City)) {
                    //    Player.localPlayer.UpgradeBuildung(clickedObject, type);
                    //}
                }
                else
                {
                    Debug.Log("Planet Clicked");
                    if(clickedObject.transform.parent.GetComponent<HexCell>().hadSpacePiratesBefore)
                    {

                        Debug.Log("space pirates before");
                    }
                    else
                    {
                        Player.localPlayer.SpawnSpacePirates(clickedObject, Array.FindIndex(hexGrid.cells, val => val.Equals(clickedObject.transform.parent.GetComponent<HexCell>())));
                    }
                }
            }
            else
            {
                Debug.Log("Nothing");
            }
        }
    }

    private void ResourceError()
    {
        Debug.Log("Not enough Resources");
        InfoBoxManager.Instance.ErrorMessageOnClient("Not enough Resources");

    }

    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }

    public void RpcUpdateVertex(int index, bool localOwner, int ownerIndex)
    {
        hexGrid.hexVertices[index].hasSettlement = true;
        hexGrid.hexVertices[index].localPlayerOwnsSettlement = localOwner;
        hexGrid.hexVertices[index].ownerIndex = ownerIndex;
        Debug.Log("Rpc Update Vertexx");
    }

    public void RpcUpdateEdge(int index, bool localOwner, int ownerIndex)
    {
        hexGrid.hexEdges[index].hasRoad = true;
        hexGrid.hexEdges[index].localPlayerOwnsRoad = localOwner;
        hexGrid.hexEdges[index].ownerIndex = ownerIndex;
        Debug.Log("Rpc Update Road");
    }

    public void RpcUpdateSpacePirateCell(int index)
    {
        hexGrid.cells[index].hasSpacePirates = true;
    }
}
