using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    //[SyncVar(hook = nameof(IsCurPlayerChanged))]
    [SyncVar]
    public bool isCurPlayer;
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

        TurnManager.Instance.AddPlayer(p);
        clientId = TurnManager.Instance.players.IndexOf(p);

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
        var diceSum = TurnManager.Instance.RollDice(out diceVal1, out diceVal2);
        RpcShowDiceOnClients(diceSum, diceVal1, diceVal2);
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
        CmdFinishTurn();
    }

    [Command]
    void CmdFinishTurn()
    {
        TurnManager.Instance.SetCurPlayerNext();
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
            uiHandler.ActivateCurPlayerUI(newPlayer);
            Debug.Log("Activate Current Player UI");
        }
        else
        {
            uiHandler.DeActivatecurPlayerUI();
            Debug.Log("DeActivate Current Player UI");
        }
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