using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
[System.Serializable]
public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    [SyncVar] public bool isCurPlayer;
    [SyncVar] public int clientId;
    [SyncVar] public Color playerColor;

    [SyncVar(hook = nameof(UpdateDarkMatterAmountUI))] public int darkMatterAmount;
    [SyncVar(hook = nameof(UpdateSpacePigAmountUI))] public int spacePigAmount;
    [SyncVar(hook = nameof(UpdateWaterAmountUI))] public int waterAmount;
    [SyncVar(hook = nameof(UpdateMetalAmountUI))] public int metalAmount;
    [SyncVar(hook = nameof(UpdateEnergyAmountUI))] public int energyAmount;

    [SyncVar]public int victoryPoints = 0;

    private UIHandler uiHandler;

    private bool hasPlacedSpacePirates;

    public int GetAllResources()
    {
        return darkMatterAmount + spacePigAmount + waterAmount + metalAmount + energyAmount;
    }
    [Server]
    public void UpdateVictoryPoints(int amount)
    {
        victoryPoints += amount;
        CheckWinCondition();
    }
    public void CheckWinCondition()
    {
        foreach (Player p in TurnManager.Instance.players)
        {
            if(p.victoryPoints >= 6)
            {
                EndGame(p.clientId);
                break;
            }
        }
    }
    [ClientRpc]
    public void EndGame(int clientId)
    {
        Time.timeScale = 0;
        uiHandler.EndGameMessage(clientId);
    }

    [Client]
    public override void OnStartClient()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        if (isLocalPlayer)
        {
            Debug.Log("Spawning local Player");
            localPlayer = this;
            CmdClientJoined(this);
        }
        else
        {
            Debug.Log("Spawning remote Player");
        }
    }

    void UpdateDarkMatterAmountUI(int oldDarkMatter,int newDarkMatter)
    {
        ResourceManager.Instance.UpdateDarkMatterUI();
    }
    void UpdateSpacePigAmountUI(int oldSpacePig, int newSpacePig)
    {
        ResourceManager.Instance.UpdateSpacePigUI();
    }
    void UpdateWaterAmountUI(int oldWater, int newWater)
    {
        ResourceManager.Instance.UpdateWaterUI();
    }
    void UpdateMetalAmountUI(int oldMetal, int newMetal)
    {
        ResourceManager.Instance.UpdateMetalUI();
    }
    void UpdateEnergyAmountUI(int oldEnergy, int newEnergy)
    {
        ResourceManager.Instance.UpdateEnergyUI();
    }

    [Client]
    public override void OnStopClient()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Local Player disconnected");
        }
        else
        {
            Debug.Log("Remote Player disconnected");
        }

        localPlayer.CmdClientLeft(this);
    } 

    [Command]
    void CmdClientJoined(Player p)
    {
        TurnManager.Instance.AddPlayer(p);
        clientId = TurnManager.Instance.players.IndexOf(p);
        TargetClientJoined(clientId);
        SetPlayerColor();
        InfoBoxManager.Instance.playerJoinMessage("Player" + p.clientId, p.clientId);

        foreach (var players in TurnManager.Instance.players)
        {
            PlayerStatsManager.Instance.setPlayerStats(players.clientId);
        }
        

        if (TurnManager.Instance.players.Count == 1)
        {
            Debug.Log("Enough Players joined - Active UI");

            GameManager.Instance.curGameState = Enums.GameState.mapGeneration;
            InfoBoxManager.Instance.writeMessage("Enough players joined - starting game");
        }
    }

    [Command]
    void CmdClientLeft(Player p)
    {
        TurnManager.Instance.RemovePlayer(p);
    }

    [Server]
    void SetPlayerColor()
    {
        switch (clientId)
        {
            case 0:
                playerColor = Color.red;
                break;
            case 1:
                playerColor = Color.green;
                break;
            case 2:
                playerColor = Color.yellow;
                break;
            case 3:
                playerColor = Color.blue;
                break;
        }
    }

    [TargetRpc]
    private void TargetClientJoined(int clientId)
    {
        uiHandler.SetLocalPlayerName("player" + clientId);
    }

    // -- Map Generation --
    [Client]
    public void GenerateMap(string seed)
    {
        CmdGenerateMap(seed);
    }

    [Command]
    private void CmdGenerateMap(string seedInput)
    {
        var seed = MapGenerator.Instance.GenerateMap(seedInput);

        RpcShowSeedOnClients(seed);
    }



    [ClientRpc]
    void RpcShowSeedOnClients(int seed)
    {
        uiHandler.UpdateSeedVal(seed);
    }

    // -- Start Game --
    [Client]
    public void StartGame()
    {
        CmdStartGame(localPlayer);
    }

    [Command]
    void CmdStartGame(Player host)
    {
        GameManager.Instance.curGameState = Enums.GameState.turnDetermization;
        TurnManager.Instance.SetCurPlayer(host);
    }

    // -- Roll Dice --
    [Client]
    public void RollDice()
    {
        CmdRollDice();
    }

    [Command]
    void CmdRollDice()
    {
        int diceVal1;
        int diceVal2;

        if (GameManager.Instance.curGameState == Enums.GameState.turnDetermization)
        {
            bool lastRollTurnDetermization;
            var diceSum = TurnManager.Instance.RollDiceTurnDetermization(out diceVal1, out diceVal2, out lastRollTurnDetermization);
            RpcShowDiceOnClients(diceSum, diceVal1, diceVal2);
            InfoBoxManager.Instance.diceRollMessage("Player" + clientId, clientId, diceSum);

            if (!lastRollTurnDetermization)
                StartCoroutine(WaitForDiceAnimation());
        }
        else
        {
            var diceSum = TurnManager.Instance.RollDice(out diceVal1, out diceVal2);
            InfoBoxManager.Instance.diceRollMessage("Player" + clientId, clientId, diceSum);
            RpcShowDiceOnClients(diceSum, diceVal1, diceVal2);

            if (diceSum == 7)
            {
                InfoBoxManager.Instance.robberActivatedMessage();
                Debug.Log("Space Pirates Time");
                ObjectPlacer.Instance.placeSpacePirates = true;
                ObjectPlacer.Instance.MarkCurrentSpacePirates();
                hasPlacedSpacePirates = false;
                GameManager.Instance.SpacePiratesRobResources();
            }
            else
            {
                GameManager.Instance.DistributeResources(diceSum);
            }
        }
    }

    [Server]
    IEnumerator WaitForDiceAnimation()
    {
        yield return new WaitForSeconds(3f);
        SrvFinishTurn();
    }

    [ClientRpc]
    void RpcShowDiceOnClients(int diceSum, int diceVal1, int diceVal2)
    {
        uiHandler.ShowDiceVal(diceSum, diceVal1, diceVal2);
    }

    // -- Turn Finishing --
    [Client]
    public void FinishTurn()
    {
        if (hasPlacedSpacePirates == false && ObjectPlacer.Instance.placeSpacePirates)
        {
            InfoBoxManager.Instance.ErrorMessageOnClient("Cannot finish turn - you must place Space Pirates");
            return;
        }
        CmdFinishTurn();
    }

    [Command]
    void CmdFinishTurn()
    {
        if (ObjectPlacer.Instance.placeSpacePirates)
        {
            ObjectPlacer.Instance.placeSpacePirates = false;
            ObjectPlacer.Instance.UnMarkCurrentSpacePirates();
            GameManager.Instance.StealResources();
        }
        SrvFinishTurn();
    }

    [Server]
    void SrvFinishTurn()
    {
        TurnManager.Instance.SetCurPlayerNext();
    }

    // -- Building Placement --
    private Enums.BuildingType currentType;
    private Vector3 buildingPos;
    private Quaternion buildingRot;
    private int objectIndex;

    [Client]
    public void PlaceBuilding(GameObject hex, Enums.BuildingType type, int objectIndex)
    {
        if (GameManager.Instance.curGameState == Enums.GameState.preGame)
        {
            if(!CheckPreGameConstraints(type))
            {
                Debug.Log("Cannot Build " + type + " - PreGrameConstraint");
                return;
            }
        }

        buildingPos = hex.transform.position;
        buildingRot = hex.transform.rotation;
        this.objectIndex = objectIndex;
        currentType = type;

        ObjectPlacer.Instance.PlacePreview(hex, type, playerColor);
        uiHandler.OpenPlacementConfirmation();
    }

    private bool hasBuiltSettlement;
    private bool hasBuiltRoad;

    [Client]
    bool CheckPreGameConstraints(Enums.BuildingType type)
    {
        if (type == Enums.BuildingType.Settlement)
        {
            if (hasBuiltSettlement) return false;
            hasBuiltSettlement = true;
        }
        else if (type == Enums.BuildingType.Road)
        {
            if (hasBuiltRoad) return false;
            hasBuiltRoad = true;
        }

        return true;
    }

    [Client]
    public void ConfirmPlacement(bool confirm)
    {
        ObjectPlacer.Instance.ConfirmPlacement();
        if (confirm)
        {
            if (GameManager.Instance.curGameState != Enums.GameState.preGame)
                UseResources(currentType); // don't use resources in preGame

            CmdSpawnBuilding(buildingPos, buildingRot, currentType, localPlayer, objectIndex);

            if (GameManager.Instance.curGameState == Enums.GameState.preGame && hasBuiltRoad && hasBuiltSettlement)
            {
                hasBuiltSettlement = false;
                hasBuiltRoad = false;
                CmdFinishTurn();
            }
        }
        else
        {
            if (currentType == Enums.BuildingType.Road)
                hasBuiltRoad = false;

            else if (currentType == Enums.BuildingType.Settlement)
                hasBuiltSettlement = false;
        }
    }

    [Command]
    void CmdSpawnBuilding(Vector3 pos, Quaternion rot, Enums.BuildingType type, Player owner, int objectIndex)
    {
        ObjectPlacer.Instance.SpawnBuilding(pos, rot, type, owner);
        RpcUpdateVertexSettlement(type, objectIndex, owner);
    }

    [ClientRpc]
    void RpcUpdateVertexSettlement(Enums.BuildingType type, int objectIndex, Player owner)
    {
        if (type == Enums.BuildingType.Settlement)
        {
            ObjectPlacer.Instance.objClicker.RpcUpdateVertex(objectIndex, localPlayer == owner, TurnManager.Instance.players.IndexOf(owner));
            UpdateVictoryPoints(1);
            CmdsetPlayerTotalSettlements(owner.clientId, 1);
        }
        else if (type == Enums.BuildingType.Road)
        {
            ObjectPlacer.Instance.objClicker.RpcUpdateEdge(objectIndex, localPlayer == owner, TurnManager.Instance.players.IndexOf(owner));
            CmdsetPlayerTotalRoads(owner.clientId, 1);
        }
    }

    [Command]
    public void CmdsetPlayerTotalSettlements(int index, int amount)
    {
        PlayerStatsManager.Instance.setPlayerTotalSettlements(index, amount);
    }

    [Command]
    public void CmdsetPlayerTotalRoads(int index, int amount)
    {
        PlayerStatsManager.Instance.setPlayerTotalRoads(index, amount);
    }

    // -- Upgrading Buildings
    Building buildingToUpgrade;
    int upgradeVertexIndex;

    [Client]
    public void UpgradeBuildung(Building building, int vertexIndex)
    {
        uiHandler.OpenUpgradingConfirmation();
        buildingToUpgrade = building;
        upgradeVertexIndex = vertexIndex;
    }

    [Client]
    public void ConfirmUpgrade(bool confirm)
    {
        if (confirm)
        {
            CmdConfirmUpgrade(buildingToUpgrade, upgradeVertexIndex);
            UseResources(Enums.BuildingType.City);
        }
    }

    [Command]
    void CmdConfirmUpgrade(Building building, int vertexIndex)
    {
        ObjectPlacer.Instance.UpgradeBuilding(building.transform.position, building.transform.rotation, building.owner);
        UpdateVictoryPoints(1);
        RpcConfirmUpgrade(building, vertexIndex);
    }

    [ClientRpc]
    void RpcConfirmUpgrade(Building settlement, int vertexIndex)
    {
        ObjectPlacer.Instance.objClicker.hexGrid.hexVertices[vertexIndex].hasCity = true;
        Destroy(settlement.gameObject);
    }

    // -- Resources --
    [Client]
    public bool HasResources(Enums.BuildingType type)
    {
        bool hasResources = false;

        switch (type)
        {
            case Enums.BuildingType.City:
                hasResources = (darkMatterAmount >= 3 && spacePigAmount >= 2);
                break;
            case Enums.BuildingType.Settlement:
                hasResources = (spacePigAmount >= 1 && waterAmount >= 1 && energyAmount >= 1 && metalAmount >= 1);
                break;
            case Enums.BuildingType.Road:
                hasResources = (metalAmount >= 1 && energyAmount >= 1);
                break;
        }

        return hasResources;
    }

    [Command]
    public void UseResources(Enums.BuildingType type)
    {
        switch (type)
        {
            case Enums.BuildingType.City:
                ChangeResourceAmount(Enums.Resources.darkMatter, -3);
                ChangeResourceAmount(Enums.Resources.spacePig, -2);
                break;
            case Enums.BuildingType.Settlement:
                ChangeResourceAmount(Enums.Resources.spacePig, -1);
                ChangeResourceAmount(Enums.Resources.water, -1);
                ChangeResourceAmount(Enums.Resources.energy, -1);
                ChangeResourceAmount(Enums.Resources.metal, -1);
                break;
            case Enums.BuildingType.Road:
                ChangeResourceAmount(Enums.Resources.energy, -1);
                ChangeResourceAmount(Enums.Resources.metal, -1);
                break;
        }
    }

    [Server]
    public void ChangeResourceAmount(Enums.Resources resource, int amount)
    {
        Debug.Log("Resources Change: " + resource.ToString() + " - " + amount);
        InfoBoxManager.Instance.ResourceMessage(clientId, amount, resource);
        switch (resource)
        {
            case Enums.Resources.darkMatter:
                darkMatterAmount += amount;
                break;
            case Enums.Resources.energy:
                energyAmount += amount;
                break;
            case Enums.Resources.metal:
                metalAmount += amount;
                break;
            case Enums.Resources.spacePig:
                spacePigAmount += amount;
                break;
            case Enums.Resources.water:
                waterAmount += amount;
                break;
        }
        PlayerStatsManager.Instance.setPlayerTotalResources(clientId, amount);
    }

    [Command]
    public void CmdCompleteTrade(int darkMatterAmount, int metalAmount, int energyAmount, int spacePigAmount, int waterAmount, int clientId)
    {
        ChangeResourceAmount(Enums.Resources.darkMatter, darkMatterAmount);
        ChangeResourceAmount(Enums.Resources.spacePig, spacePigAmount);
        ChangeResourceAmount(Enums.Resources.water, waterAmount);
        ChangeResourceAmount(Enums.Resources.metal, metalAmount);
        ChangeResourceAmount(Enums.Resources.energy, energyAmount);
        InfoBoxManager.Instance.playerTradedMessage("Player" + clientId, clientId);
    }

    // -- UI Updates --
    [Client]
    public void UpdateGameState(string curGameState)
    {
        uiHandler.UpdateCurStateVal(curGameState);
    }

    [Client]
    public void UpdateCurPlayer(string curPlayer)
    {
        uiHandler.UpdateCurPlayerVal(curPlayer);
    }

    [Client]
    public void SwitchGameStateUI(Enums.GameState newState)
    {
        uiHandler.SwitchGameStateUI(newState);
    }

    [Client]
    public void SwitchCurPlayerUI(Player newPlayer)
    {
        if (this == newPlayer)
        {
            uiHandler.ActivateCurPlayerUI();
            Debug.Log("Activate Current Player UI");
        }
        else
        {
            uiHandler.DeActivateCurPlayerUI();
            Debug.Log("DeActivate Current Player UI");
        }
    }

    // -- Space Pirates --
    [Client]
    public void SpawnSpacePirates(GameObject clickedPlanet, int cellIndex)
    {
        hasPlacedSpacePirates = true;
        CmdSpawnSpacePirates(clickedPlanet.transform.position, cellIndex);
    }

    [Command]
    private void CmdSpawnSpacePirates(Vector3 pos, int cellIndex)
    {
        //if (ObjectPlacer.Instance.objClicker.hexGrid.cells[cellIndex].hadSpacePiratesPreviously)
        //{
        //    Debug.Log("This cell had space pirates previously");
        //} 
        //else
        //{
            if (ObjectPlacer.Instance.SpawnSpacePirates(pos))
                RpcSpawnSpacePirates(cellIndex);
        //}
    }

    [ClientRpc]
    private void RpcSpawnSpacePirates(int index)
    {
        ObjectPlacer.Instance.objClicker.RpcUpdateSpacePirateCell(index);
    }

    #region Temp / Debug
    [Client]
    public void GetCurPlayer()
    {
        CmdGetCurPlayer();
    }

    [Command]
    private void CmdGetCurPlayer()
    {
        if (TurnManager.Instance.curPlayer != null)
        {
            //TurnManager.Instance.SetCurPlayer(TurnManager.Instance.players[0]);
            TargetGetCurPlayer(TurnManager.Instance.curPlayer);
        }
    }

    [TargetRpc]
    private void TargetGetCurPlayer(Player p)
    {
        Debug.Log($"Current Player: {p.clientId}");
    }

    #endregion
}