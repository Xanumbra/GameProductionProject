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

            var ownerIndices = cell.hexVertices.Where(v => v.hasSettlement).Select(v => v.ownerIndex);
            
            if (!cell.hasSpacePirates)
            {
                foreach (var i in ownerIndices)
                {
                    TurnManager.Instance.players[i].ChangeResourceAmount(cell.cellResourceType, 1);
                }
            }
        }
    }
}
