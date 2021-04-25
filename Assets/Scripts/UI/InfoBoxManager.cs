using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoxManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static InfoBoxManager _instance;

    public static Text infoBox;
    public Text infoBoxHolder;
    public static InfoBoxManager Instance { get { return _instance; } }
    void Awake()
    {
       if( _instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        infoBox = infoBoxHolder;
    }
    private void Start()
    {
        writeMessage("Hello, world!");
        writeMessage("Test Message");
    }

    static void writeMessage(string message)
    {
        infoBox.text += "\n" + message;
    }
    static void playerTurnMessage(string playername)
    {
        infoBox.text += "\nIt's "+ playername +"'s turn.";
    }
}
