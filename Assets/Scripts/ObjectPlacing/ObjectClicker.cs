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
                    if (clickedObject.GetComponent<HexEdges>().hasRoad) return;
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
                    if (clickedObject.GetComponent<HexVertices>().hasCity) return;
                    type = Enums.BuildingType.Settlement;
                    var hexVertex = clickedObject.GetComponent<HexVertices>();


                    if (hexVertex.IsSettlementValid())
                    {
                        if (hexVertex.hasBuilding && !hexVertex.hasCity)
                        {
                            Debug.Log("Settlement Clicked");

                            if (GameManager.Instance.curGameState != Enums.GameState.preGame && Player.localPlayer.HasResources(Enums.BuildingType.City)
                                && hexVertex.ownerIndex == TurnManager.Instance.players.IndexOf(Player.localPlayer))
                            {
                                Debug.Log("Upgrading Settlement");
                                var clickedBuilding = FindObjectsOfType<Building>().Where(b => b.transform.position == clickedObject.transform.position).Select(b => b).ElementAt(0);
                                Player.localPlayer.UpgradeBuildung(clickedBuilding, hexGrid.hexVertices.IndexOf(hexVertex));
                            }
                        }
                        else
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
                }
                else /*if (clickedObject.c)*/
                {
                    Debug.Log("Planet Clicked");
                    if(clickedObject.transform.GetComponentInParent<HexCell>().hadSpacePiratesBefore)
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
        hexGrid.hexVertices[index].hasBuilding = true;
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
