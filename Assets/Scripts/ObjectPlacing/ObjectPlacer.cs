using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ObjectPlacer : NetworkBehaviour
{
    private static ObjectPlacer _instance;
    public static ObjectPlacer Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        objClicker = GetComponent<ObjectClicker>();
    }

    public GameObject redCityPrefab;
    public GameObject greenCityPrefab;
    public GameObject yellowCityPrefab;
    public GameObject blueCityPrefab;
    public GameObject redRoadPrefab;
    public GameObject greenRoadPrefab;
    public GameObject yellowRoadPrefab;
    public GameObject blueRoadPrefab;
    public GameObject redSettlementPrefab;
    public GameObject greenSettlementPrefab;
    public GameObject yellowSettlementPrefab;
    public GameObject blueSettlementPrefab;
    public Transform buildingsParent;

    public GameObject spacePiratePrefab;

    [SyncVar] public bool placeSpacePirates;

    public ObjectClicker objClicker;

    private GameObject currentBuilding;
    private bool currentlyPreviewing;

    private GameObject curSpacePirates;

    [Client]
    public void PlacePreview(GameObject hexObject, Enums.BuildingType type, Color playerColor)
    {
        currentlyPreviewing = true;
        objClicker.enabled = false;

        var prefab = GetColoredPrefab(type, playerColor);
        prefab.GetComponent<Building>().type = type;
        currentBuilding = Instantiate(prefab, buildingsParent);
        
        
        var rot = hexObject.transform.rotation;
        rot *= Quaternion.Euler(0, 90, 0);

        currentBuilding.transform.position = hexObject.transform.position + new Vector3(0, 8, 0);

        currentBuilding.transform.rotation = rot;
        StartCoroutine(AnimatePreviewBuilding());
    }

    private GameObject GetColoredPrefab(Enums.BuildingType type, Color playerColor)
    {
        if (type == Enums.BuildingType.Settlement)
        {
            if (playerColor == Color.red) return redSettlementPrefab;
            if (playerColor == Color.green) return greenSettlementPrefab;
            if (playerColor == Color.yellow) return yellowSettlementPrefab;
            if (playerColor == Color.blue) return blueSettlementPrefab;  
        }
        else if (type == Enums.BuildingType.City)
        {
            if (playerColor == Color.red) return redCityPrefab;
            if (playerColor == Color.green) return greenCityPrefab;
            if (playerColor == Color.yellow) return yellowCityPrefab;
            if (playerColor == Color.blue) return blueCityPrefab;
        }
        else
        {
            if (playerColor == Color.red) return redRoadPrefab;
            if (playerColor == Color.green) return greenRoadPrefab;
            if (playerColor == Color.yellow) return yellowRoadPrefab;
            if (playerColor == Color.blue) return blueRoadPrefab;

        }

        return null;
    }

    [Client]
    private IEnumerator AnimatePreviewBuilding()
    {
        int direction = 0; // 0 --> down || 1 --> up

        while (currentlyPreviewing)
        {
            if (currentBuilding.transform.position.y >= 6 && direction == 0)
            {
                currentBuilding.transform.position -= new Vector3(0, 0.05f, 0);
                if (currentBuilding.transform.position.y == 6) direction = 1;
            }

            if (currentBuilding.transform.position.y <= 8 && direction == 1)
            {
                currentBuilding.transform.position += new Vector3(0, 0.05f, 0);
                if (currentBuilding.transform.position.y == 8) direction = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    [Client]
    public void ConfirmPlacement()
    {
        objClicker.enabled = true;
        currentlyPreviewing = false;

        Destroy(currentBuilding);
    }

    [Server]
    public void SpawnBuilding(Vector3 pos, Quaternion rot, Enums.BuildingType type, Player owner)
    {
        var prefab = GetColoredPrefab(type, owner.playerColor);

        var newObj = Instantiate(prefab, buildingsParent);

        newObj.GetComponent<Building>().owner = owner;
        newObj.GetComponent<Building>().parent = buildingsParent;
        newObj.GetComponent<Building>().type = type;
        newObj.transform.position = pos;
        rot *= Quaternion.Euler(0, 90, 0);
        newObj.transform.rotation = rot;


        NetworkServer.Spawn(newObj);
    }

    // -- Upgrade Building --
    [Server]
    public void UpgradeBuilding(Vector3 pos, Quaternion rot, Player owner)
    {
        SpawnBuilding(pos, rot, Enums.BuildingType.City, owner);
    }

    // -- Space Pirates --
    [Server]
    public bool SpawnSpacePirates(Vector3 pos)
    {
        if (!placeSpacePirates)
        {
            Debug.Log("Cannot place space Pirates now");
            return false;
        }

        if (curSpacePirates != null)
        {
            RpcUpdateSpacePirateCell();

            Destroy(curSpacePirates);
        }

        var newObj = Instantiate(spacePiratePrefab);

        curSpacePirates = newObj;

        newObj.transform.position = pos + new Vector3(0, 8, 0);

        NetworkServer.Spawn(newObj);
        return true;
    }

    [ClientRpc]
    private void RpcUpdateSpacePirateCell()
    {
        foreach (var cell in objClicker.hexGrid.cells)
        {
            cell.hasSpacePirates = false;
        }
    }

    [ClientRpc]
    public void MarkCurrentSpacePirates()
    {
        //if (curSpacePirates != null) // curSpacePirates is null if no pirates have been been paced yet
        //{
            foreach (var cell in objClicker.hexGrid.cells)
            {
                if (cell.hasSpacePirates) cell.hadSpacePiratesBefore = true;
            }
        //}
    }

    [ClientRpc]
    public void UnMarkCurrentSpacePirates()
    {
        //if (curSpacePirates != null) // curSpacePirates is null if no pirates have been been paced yet
        //{
            foreach (var cell in objClicker.hexGrid.cells)
            {
                cell.hadSpacePiratesBefore = false;
            }
       // }
    }


}
