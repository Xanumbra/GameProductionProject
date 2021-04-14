using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    [SyncVar] public int clientId;

    private UIHandler uiHandler;

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
    }

    [Command]
    private void CmdClientJoined(Player p)
    {
        var rnd = new System.Random();
        clientId = NetworkServer.connections.Count;

        TurnManager.Instance.AddPlayer(p);

        if (TurnManager.Instance.players.Count == 2)
        {
            Debug.Log("Enough Players joined - Active UI");

            GameManager.Instance.curGameState = Enums.GameState.mapGeneration;
            
            //RpcActivateMapGenerationUI();
        }
    }

    //[ClientRpc]
    //private void RpcActivateMapGenerationUI()
    //{
    //    if (isServer)
    //    {
    //        uiHandler.ActivateMapGenerationUIHost();
    //    }
    //}

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
        CmdStartGame();
    }

    [Command]
    void CmdStartGame()
    {
        GameManager.Instance.curGameState = Enums.GameState.turnDetermization;
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
        FindObjectOfType<UIHandler>().SwitchGameStateUI(newState);
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
        if (TurnManager.Instance.curPlayer == null)
        {
            TurnManager.Instance.SetCurPlayer(TurnManager.Instance.players[0]);
        }

        TargetGetCurPlayer(TurnManager.Instance.curPlayer);
    }

    [TargetRpc]
    private void TargetGetCurPlayer(Player p)
    {
        Debug.Log($"Current Player: {p.clientId}");
    }

    #endregion
}
