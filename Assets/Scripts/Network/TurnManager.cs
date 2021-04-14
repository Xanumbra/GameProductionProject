using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class SyncListPlayer : SyncList<Player> { }

public class TurnManager : NetworkBehaviour
{
    private static TurnManager _instance;
    public static TurnManager Instance { get { return _instance; } }
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

    [SyncVar(hook = nameof(UpdateCurPlayer))]
    public Player curPlayer;

    public SyncListPlayer players = new SyncListPlayer();

    [Server]
    public void AddPlayer(Player p)
    {
        players.Add(p);
        Debug.Log("Added Player");
        Debug.Log($"Player count: {players.Count}");
    }

    [Server]
    public void RemovePlayer(Player p)
    {
        players.Add(p);
        Debug.Log("Removed Player");
        Debug.Log($"Player count: {players.Count}");
    }

    [Server]
    public void SetCurPlayer(Player p)
    {
        curPlayer = p;
        Debug.Log($"Switched curPlayer");
    }

    void UpdateCurPlayer(Player oldPlayer, Player newPlayer)
    {
        var s = "player " + players.IndexOf(curPlayer);
        Debug.Log($"Current Player changed to {s}");
        Player.localPlayer.UpdateGameState(s);
    }
}
