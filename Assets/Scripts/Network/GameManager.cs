using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

    void Start()
    {
        curGameState = Enums.GameState.waitingForPlayers;
    }

    // Sync var Hook --> Called on Client when SyncVar changes
    void UpdateGameState(Enums.GameState oldState, Enums.GameState newState)
    {
        Debug.Log($"Game State changed from {oldState.ToString()} to {newState.ToString()}");
        Player.localPlayer.UpdateGameState(newState.ToString());
        Player.localPlayer.SwitchGameStateUI(newState);
    }
}
