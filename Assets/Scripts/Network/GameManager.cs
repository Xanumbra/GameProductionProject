using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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
    }

    [SyncVar(hook = nameof(UpdateGameState))]
    public Enums.GameState curGameState;

    public SoundManager soundManager;

    private HexGrid hexGrid;

    void Start()
    {
        curGameState = Enums.GameState.waitingForPlayers;
        hexGrid = FindObjectOfType<HexGrid>();
    }

    // Sync var Hook --> Called on Client when SyncVar changes
    void UpdateGameState(Enums.GameState oldState, Enums.GameState newState)
    {
        Debug.Log($"Game State changed from {oldState.ToString()} to {newState.ToString()}");

        Player.localPlayer.UpdateGameState(newState.ToString());
        Player.localPlayer.SwitchGameStateUI(newState);

        ShowNewGameStateMessage(newState);
    }

    void ShowNewGameStateMessage(Enums.GameState state)
    {
        switch (state)
        {
            case Enums.GameState.waitingForPlayers:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>Waiting for Players</b> - Not enough players joined your game");
                soundManager.ChangeAudioClip("WaitingForPlayers");
                break;
            case Enums.GameState.mapGeneration:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>Map Generation</b> - Host can generate random map now");
                break;
            case Enums.GameState.turnDetermization:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>Turn Determization</b> - Everybody rolls the dice - the player with the highest number will be first, the player with the lowest number will be last");
                soundManager.ChangeAudioClip("InGame");
                break;
            case Enums.GameState.preGame:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>Pre Game</b> - Everyone can place a Settlement and a Road on his turn");
                break;
            case Enums.GameState.inGame:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>In Game</b> - Roll dice to gain resources to build something");
                break;   
            case Enums.GameState.postGame:
                InfoBoxManager.Instance.ErrorMessageOnClient("<b>Post Game</b> - Congratulations, the game is over");
                break;

        }
    }

    [Server]
    public void DistributeResources(int diceSum)
    {
        var matchingCells = hexGrid.cells.Where(c => c.cellDiceNumber == diceSum).Select(c => c);

        foreach (var cell in matchingCells)
        {
            Debug.Log(cell.cellID + "---" + cell.cellResourceType);

            var settlementOwnerIndices = cell.hexVertices.Where(v => v.hasBuilding && !v.hasCity).Select(v => v.ownerIndex);
            var cityOwnerIndices = cell.hexVertices.Where(v => v.hasCity).Select(v => v.ownerIndex);
                
            if (!cell.hasSpacePirates)
            {

                foreach (var i in settlementOwnerIndices)
                {
                    var p = TurnManager.Instance.players[i];
                    p.ChangeResourceAmount(cell.cellResourceType, 1);
                }

                foreach (var i in cityOwnerIndices)
                {
                    var p = TurnManager.Instance.players[i];
                    p.ChangeResourceAmount(cell.cellResourceType, 2);
                }
            }
            else
            {
                foreach (var i in settlementOwnerIndices)
                {
                    InfoBoxManager.Instance.writeMessage("Cannot gain Resource " + cell.cellResourceType + " because of SpacePirates");
                }
                foreach (var i in cityOwnerIndices)
                {
                    InfoBoxManager.Instance.writeMessage("Cannot gain Resource " + cell.cellResourceType + " because of SpacePirates");
                }
            }
        }
        ResourceManager.Instance.UpdateResourceUI();
    }

    [Server]
    public void StealResources()
    {
        foreach (var cell in hexGrid.cells)
        {
            if (cell.hasSpacePirates)
            {
                var playersWithBuilding = new List<Player>();
                foreach (var vertex in cell.hexVertices)
                {
                    if (vertex.hasBuilding) playersWithBuilding.Add(TurnManager.Instance.players[vertex.ownerIndex]);
                }

                foreach (var player in playersWithBuilding)
                {
                    if (player != TurnManager.Instance.curPlayer)
                    {
                        Debug.Log("Player " + TurnManager.Instance.curPlayer.clientId + " steals resource " + cell.cellResourceType + " from player " + player.clientId);
                        switch (cell.cellResourceType)
                        {
                            case Enums.Resources.darkMatter:
                                if (player.darkMatterAmount <= 0)
                                {
                                    Debug.Log("Cannot steal!");
                                    return;
                                }
                                break;
                            case Enums.Resources.energy:
                                if (player.energyAmount <= 0)
                                {
                                    Debug.Log("Cannot steal!");
                                    return;
                                }
                                break;
                            case Enums.Resources.metal:
                                if (player.metalAmount <= 0)
                                {
                                    Debug.Log("Cannot steal!");
                                    return;
                                }
                                break;
                            case Enums.Resources.spacePig:
                                if (player.spacePigAmount <= 0)
                                {
                                    Debug.Log("Cannot steal!");
                                    return;
                                }
                                break;
                            case Enums.Resources.water:
                                if (player.waterAmount <= 0)
                                {
                                    Debug.Log("Cannot steal!");
                                    return;
                                }
                                break;
                            case Enums.Resources.sun:
                                Debug.Log("Cannot steal when at sun!");
                                return;
                        }

                        player.ChangeResourceAmount(cell.cellResourceType, -1);
                        TurnManager.Instance.curPlayer.ChangeResourceAmount(cell.cellResourceType, 1);
                    }
                }
                
            }
		}
        ResourceManager.Instance.UpdateResourceUI();
    }

    [Server]
    public void SpacePiratesRobResources()
    {
        /*var validPlayers = TurnManager.Instance.players.Where(p => p.GetAllResources() > 7).Select(p => p);

        foreach (var p in validPlayers)
        {
            int resourceSum = p.GetAllResources() / 2;
            Random.InitState(System.Environment.TickCount);

            while (p.GetAllResources() > resourceSum)
            {
                
                var typeIndex = Random.Range(0, 4);
                var type = Enums.Resources.darkMatter;
                var curAmount = 0;

                switch (typeIndex)
                {
                    case 0:
                        type = Enums.Resources.darkMatter; 
                        curAmount = p.darkMatterAmount;
                        break;
                    case 1:
                        type = Enums.Resources.water;
                        curAmount = p.waterAmount;
                        break;
                    case 2:
                        type = Enums.Resources.energy;
                         curAmount = p.energyAmount;
                        break;
                    case 3:
                        type = Enums.Resources.metal;
                        curAmount = p.metalAmount;
                        break;
                    case 4:
                        type = Enums.Resources.spacePig;
                        curAmount = p.spacePigAmount;
                        break;
                }

                var reduceAmount = Random.Range(1, (curAmount / 2) * (-1));
                p.ChangeResourceAmount(type, reduceAmount);
            }
        }*/
    }
}
