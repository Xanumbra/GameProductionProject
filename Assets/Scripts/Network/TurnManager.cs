using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

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

        diceRoll = GetComponent<DiceRoll>();
    }

    //[SyncVar]
    [SyncVar(hook = nameof(UpdateCurPlayer))]
    public Player curPlayer;
    public SyncListPlayer players = new SyncListPlayer();

    private Dictionary<Player, int> turnDetermizationDiceRolls = new Dictionary<Player, int>();

    private DiceRoll diceRoll;

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
        if (curPlayer != null) curPlayer.isCurPlayer = false;
        curPlayer = p;
        p.isCurPlayer = true;
        Debug.Log($"Switched curPlayer");
    }

    [Server]
    public void SetCurPlayerNext()
    {
        var i = players.IndexOf(curPlayer);
        if (i == players.Count - 1) i = 0;
        else i++;
        SetCurPlayer(players[i]);
    }

    //Sync var Hook --> Called on Client when SyncVar changes
    void UpdateCurPlayer(Player oldPlayer, Player newPlayer)
    {
        var s = "player " + curPlayer.clientId;
        Debug.Log($"Current Player changed to {s}");
        Player.localPlayer.UpdateCurPlayer(s);
        Player.localPlayer.SwitchCurPlayerUI(curPlayer);
    }

    // -- Dice Rolling
    [Server]
    public int RollDice(out int diceVal1, out int diceVal2)
    {
        var result = diceRoll.RollDice(out diceVal1, out diceVal2);

        Debug.Log($"Roll Dice Result: {diceVal1} + {diceVal2} = {result}");

        turnDetermizationDiceRolls[curPlayer] = result;
        if (turnDetermizationDiceRolls.Count == players.Count) // every player rolled
        {
            Debug.Log("Finalizing Turn Order");
            DetermineStartOrder();
        }

        return result;
    }

    [Server]
    void DetermineStartOrder()
    {
        var sortedDiceRolls = from entry in turnDetermizationDiceRolls orderby entry.Value descending select entry;
        players.Clear();
        foreach (var diceRollKV in sortedDiceRolls)
        {
            Debug.Log($"Player {diceRollKV.Key.clientId} rolled {diceRollKV.Value}");
            players.Add(diceRollKV.Key);
        }

        turnDetermizationDiceRolls.Clear();

        StartCoroutine(WaitForDiceRolling());

    }

    [Server]
    IEnumerator WaitForDiceRolling()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.curGameState = Enums.GameState.preGame;
        SetCurPlayer(players[0]);

    }
}
