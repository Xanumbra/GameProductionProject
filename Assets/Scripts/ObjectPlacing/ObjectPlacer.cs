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

    public GameObject cityPrefab;
    public GameObject roadPrefab;
    public GameObject settlementPrefab;
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

        switch (type)
        {
            case Enums.BuildingType.City:
                currentBuilding = Instantiate(cityPrefab, buildingsParent);
                break;
            case Enums.BuildingType.Road:
                currentBuilding = Instantiate(roadPrefab, buildingsParent);
                break;
            case Enums.BuildingType.Settlement:
                currentBuilding = Instantiate(settlementPrefab, buildingsParent);
                break;
        }

        currentBuilding.GetComponent<MeshRenderer>().material.color = playerColor;
        currentBuilding.transform.position = hexObject.transform.position + new Vector3(0, 8, 0);
        currentBuilding.transform.rotation = hexObject.transform.rotation;
        StartCoroutine(AnimatePreviewBuilding());
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
        var newObj = gameObject;
        switch (type)
        {
            case Enums.BuildingType.City:
                newObj = Instantiate(cityPrefab, buildingsParent);
                break;
            case Enums.BuildingType.Road:
                newObj = Instantiate(roadPrefab, buildingsParent);
                break;
            case Enums.BuildingType.Settlement:
                newObj = Instantiate(settlementPrefab, buildingsParent);
                break;
        }

        newObj.GetComponent<Building>().owner = owner;
        newObj.GetComponent<Building>().parent = buildingsParent;
        newObj.transform.position = pos;
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
        if (curSpacePirates != null) // curSpacePirates is null if no pirates have been been paced yet
        {
            foreach (var cell in objClicker.hexGrid.cells)
            {
                if (cell.hasSpacePirates) cell.hadSpacePiratesBefore = true;
            }
        }
    }

    [ClientRpc]
    public void UnMarkCurrentSpacePirates()
    {
        if (curSpacePirates != null) // curSpacePirates is null if no pirates have been been paced yet
        {
            foreach (var cell in objClicker.hexGrid.cells)
            {
                cell.hadSpacePiratesBefore = false;
            }
        }
    }


}
