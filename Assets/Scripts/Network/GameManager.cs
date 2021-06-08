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
                    InfoBoxManager.Instance.ResourceMessage("Player" + p.clientId, p.clientId, 1, cell.cellResourceType);
                }

                foreach (var i in cityOwnerIndices)
                {
                    var p = TurnManager.Instance.players[i];
                    p.ChangeResourceAmount(cell.cellResourceType, 2);
                    InfoBoxManager.Instance.ResourceMessage("Player" + p.clientId, p.clientId, 2, cell.cellResourceType);
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
    }
}
