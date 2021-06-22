using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsManager : NetworkBehaviour
{
    private static PlayerStatsManager _instance;

    public TMP_Text statsText0;
    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text statsText3;

    public TMP_Text totalResourceText0;
    public TMP_Text totalResourceText1;
    public TMP_Text totalResourceText2;
    public TMP_Text totalResourceText3;

    public TMP_Text totalSettlementText0;
    public TMP_Text totalSettlementText1;
    public TMP_Text totalSettlementText2;
    public TMP_Text totalSettlementText3;

    public TMP_Text totalRoadText0;
    public TMP_Text totalRoadText1;
    public TMP_Text totalRoadText2;
    public TMP_Text totalRoadText3;

    public GameObject userStatsPanel1;
    public GameObject userStatsPanel2;
    public GameObject userStatsPanel3;
    public GameObject userStatsPanel4;

    public GameObject userStatsPanel1ColorContainer;
    public GameObject userStatsPanel2ColorContainer;
    public GameObject userStatsPanel3ColorContainer;
    public GameObject userStatsPanel4ColorContainer;

    public TMP_Text victoryPoints0;
    public TMP_Text victoryPoints1;
    public TMP_Text victoryPoints2;
    public TMP_Text victoryPoints3;

    private int totalResource0 = 0;
    private int totalResource1 = 0;
    private int totalResource2 = 0;
    private int totalResource3 = 0;

    private Outline panelOutline;
    private Color panelColor;

    public int totalSettlement0 = 0;
    public int totalSettlement1 = 0;
    public int totalSettlement2 = 0;
    public int totalSettlement3 = 0;

    public int totalRoad0 = 0;
    public int totalRoad1 = 0;
    public int totalRoad2 = 0;
    public int totalRoad3 = 0;



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
            statsText0.text = "Player 0";
                userStatsPanel1ColorContainer.SetActive(true);
                break;
        case 1:
            statsText1.text = "Player 1";
                userStatsPanel2ColorContainer.SetActive(true);
                break;
        case 2:
            statsText2.text = "Player 2";
                userStatsPanel3ColorContainer.SetActive(true);
                break;
        case 3:
            statsText3.text = "Player 3";
                userStatsPanel4ColorContainer.SetActive(true);

                break;
        default:
            Debug.Log("Invalid player index");
            break;
      }
    }

    [ClientRpc]
    public void setCurrentPlayer(int index) {
        if (panelOutline) {
            panelOutline.effectColor = new Color(0,0,0,1);
        }
         switch (index)
      {
        case 0:
            panelOutline = userStatsPanel1.GetComponent<Outline>();
            break;
        case 1:
            panelOutline = userStatsPanel2.GetComponent<Outline>();
                break;
        case 2:
            panelOutline = userStatsPanel3.GetComponent<Outline>();
            break;
        case 3:
            panelOutline = userStatsPanel4.GetComponent<Outline>();
            break;
        default:
            Debug.Log("Invalid player index");
            break;
      }

        panelOutline.effectColor = new Color(0, 1, 0, 0.5f);
    }

    [ClientRpc]
    public void setPlayerTotalResources(int index, int amount) {
        switch (index)
      {
        case 0:
            totalResource0 += amount;
            totalResourceText0.text = totalResource0.ToString();
            break;
        case 1:
            totalResource1 += amount;
            totalResourceText1.text = totalResource1.ToString();
            break;
        case 2:
            totalResource2 += amount;
            totalResourceText2.text = totalResource2.ToString();
            break;
        case 3:
            totalResource3 += amount;
            totalResourceText3.text = totalResource3.ToString();
            break;
        default:
            Debug.Log("Invalid player index");
            break;
      }
    }

    
    [ClientRpc]
    public void setPlayerTotalSettlements(int index, int amount) {
        switch (index)
      {
        case 0:
            totalSettlement0 += amount;
            totalSettlementText0.text = totalSettlement0.ToString();
            break;
        case 1:
            totalSettlement1 += amount;
            totalSettlementText1.text = totalSettlement1.ToString();
            break;
        case 2:
            totalSettlement2 += amount;
            totalSettlementText2.text = totalSettlement2.ToString();
            break;
        case 3:
            totalSettlement3 += amount;
            totalSettlementText3.text = totalSettlement3.ToString();
            break;
        default:
            Debug.Log("Invalid player index");
            break;
      }
    }

    [ClientRpc]
    public void setPlayerTotalRoads(int index, int amount) {
        switch (index)
      {
        case 0:
            totalRoad0 += amount;
            totalRoadText0.text = totalRoad0.ToString();
            break;
        case 1:
            totalRoad1 += amount;
            totalRoadText1.text = totalRoad1.ToString();
            break;
        case 2:
            totalRoad2 += amount;
            totalRoadText2.text = totalRoad2.ToString();
            break;
        case 3:
            totalRoad3 += amount;
            totalRoadText3.text = totalRoad3.ToString();
            break;
        default:
            Debug.Log("Invalid player index");
            break;
      }
    }

    public void SetPlayerVictoryPoints(int index, int amount)
    {
        switch (index)
        {
            case 0:
                victoryPoints0.text = amount.ToString();
                break;
            case 1:
                victoryPoints1.text = amount.ToString();
                break;
            case 2:
                victoryPoints2.text = amount.ToString();
                break;
            case 3:
                victoryPoints3.text = amount.ToString();
                break;
            default:
                Debug.Log("Invalid player index");
                break;
        }
        Debug.Log("Player" + index + "got victory point");
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
