using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsManager : NetworkBehaviour
{
    private static PlayerStatsManager _instance;

    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text statsText3;
    public TMP_Text statsText4;

    public static PlayerStatsManager Instance { get { return _instance; }}

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
    public void setPlayerStats(int index) {
        switch (index)
      {
        case 0:
            statsText1.text = "Player 0";
            break;
        case 1:
            statsText2.text = "Player 1";
            break;
        case 2:
            statsText3.text = "Player 2";
            break;
        case 3:
            statsText4.text = "Player 3";
            break;
        default:
            Debug.Log("Strange player index");
            break;
      }
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
