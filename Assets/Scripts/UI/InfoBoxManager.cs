using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoxManager : NetworkBehaviour
{
    private static InfoBoxManager _instance;

    public Text infoBox;

    private const string BoldColorEnding = "</b></color>";
    public static InfoBoxManager Instance { get { return _instance; } }
    void Awake()
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
    [ClientRpc]
    public void writeMessage(string message)
    {
        infoBox.text += "\n" + message;
    }
    [Client]
    public void ClientWriteMessage(string message)
    {
        infoBox.text += "\n" + message;
    }
    [ClientRpc]
    public void playerTurnMessage(string playerName, int order)
    {
        playerName = SetPlayerNameBoldColorWithS(playerName, order);
        infoBox.text += "\nIt's " + playerName + " turn.";
    }

    [ClientRpc]
    public void playerJoinMessage(string playerName, int order)
    {
        playerName = SetPlayerNameBoldColor(playerName, order);
        infoBox.text += "\n"+ playerName + " joined the game";
    }

    [ClientRpc]
    public void playerTradedMessage(string playerName, int order)
    {
        playerName = SetPlayerNameBoldColor(playerName, order);
        infoBox.text += "\n" + playerName + " traded with the bank.";
    }
    [ClientRpc]
    public void diceRollMessage(string playerName, int order, int diceTotal)
    {
        playerName = SetPlayerNameBoldColor(playerName, order);
        infoBox.text += "\n" + playerName + " rolled " + diceTotal + ".";
    }
    [ClientRpc]
    public void robberActivatedMessage()
    {
        infoBox.text += "<color=black>Space Pirates have been activated!</color>";
    }
    [Client]
    public void ErrorMessageOnClient(string message)
    {
        infoBox.text += "\n" + message;
    }

    [ClientRpc]
    public void ResourceMessage(int receiverId, int amount, Enums.Resources type)
    {
        var playerName = SetPlayerNameBoldColor("Player " + receiverId, receiverId);
        var s = " <b>received</b> ";
        if (amount < 0) s = " <b>lost</b> ";
        infoBox.text += "\n" + playerName + s + amount + " " + type;
    }

    string SetPlayerNameBoldColor(string playerName, int order)
    {
        string coloredName = playerName;
        switch (order)
        {
            case 0:
                coloredName = "<color=red><b>" + playerName + BoldColorEnding;
                break;
            case 1:
                coloredName = "<color=green><b>" + playerName + BoldColorEnding;
                break;
            case 2:
                coloredName = "<color=yellow><b>" + playerName + BoldColorEnding;
                break;
            case 3:
                coloredName = "<color=blue><b>" + playerName + BoldColorEnding;
                break;
        }
        return coloredName;
    }
    string SetPlayerNameBoldColorWithS(string playerName, int order)
    {
        string coloredName = playerName;
        switch (order)
        {
            case 0:
                coloredName = "<color=red><b>" + playerName + "'s" + BoldColorEnding;
                break;
            case 1:
                coloredName = "<color=green><b>" + playerName + "'s" + BoldColorEnding;
                break;
            case 2:
                coloredName = "<color=yellow><b>" + playerName + "'s" + BoldColorEnding;
                break;
            case 3:
                coloredName = "<color=blue><b>" + playerName + "'s" + BoldColorEnding;
                break;
        }
        return coloredName;
    }

}
