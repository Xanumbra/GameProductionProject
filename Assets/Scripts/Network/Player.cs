using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    [SyncVar] public int clientId;

    [Client]
    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
            Debug.Log("Spawning local Player");

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

        TurnManager.singleton.AddPlayer(p);
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
        if (TurnManager.singleton.curPlayer == null)
        {
            TurnManager.singleton.SetCurPlayer(TurnManager.singleton.players[0]);
        }

        TargetGetCurPlayer(TurnManager.singleton.curPlayer);
    }

    [TargetRpc]
    private void TargetGetCurPlayer(Player p)
    {
        Debug.Log($"Current Player: {p.clientId}");
    }

    #endregion
}
