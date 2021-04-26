using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectClicker : NetworkBehaviour
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
                    // TODO -- ADD PLACEMENT RULES FOR ROADS
                    type = Enums.BuildingType.Road;
                    var hexEdge = clickedObject.GetComponent<HexEdges>();
                    hexEdge.hasRoad = true;
                    Player.localPlayer.PlaceBuilding(clickedObject, type, hexGrid.hexEdges.IndexOf(hexEdge));

                }
                else if (clickedObject.name.Contains("Vertex"))
                {
                    type = Enums.BuildingType.Settlement;
                    var hexVertex = clickedObject.GetComponent<HexVertices>();
                    if (hexVertex.IsBuildingValid())
                    {
                        hexVertex.hasSettlement = true;
                        Player.localPlayer.PlaceBuilding(clickedObject, type, hexGrid.hexVertices.IndexOf(hexVertex));
                    }
                    else
                    {
                        Debug.Log("Cannot Build here - Neighbor Collision");
                    }
                }
            }
            else
            {
                Debug.Log("Nothing");
            }
        }
    }
    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }

    public void RpcUpdateVertex(int index)
    {
        hexGrid.hexVertices[index].hasSettlement = true;
        Debug.Log("Rpc Update Vertexx");
    }


    public void RpcUpdateEdge(int index)
    {
        hexGrid.hexEdges[index].hasRoad = true;
        Debug.Log("Rpc Update Road");
    }

}
