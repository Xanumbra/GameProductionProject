using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIHandler : MonoBehaviour
{
    public TMP_Text localPlayerName;

    public GameObject mapGenerationStateBtns;
    public GameObject startGameBtn;
    public GameObject seedInputParent;
    InputField seedInput;

    public GameObject curPlayerParent;
    public TMP_Text curPlayerVal;
    public TMP_Text curStateVal;

    public GameObject rollDiceBtn;
    public TMP_Text diceValLocal;
    public TMP_Text diceValRemote;
    private bool isDiceRolling = false;

    public GameObject clickManager;

    public GameObject finishTurnBtn;

    public GameObject placementConfirmationMenu;
    public GameObject upgradeConfirmationMenu;
    public GameObject tradePanel;

    private void Start()
    {
        seedInput = seedInputParent.GetComponentInChildren<InputField>();
    }

    // -- Button Methods --
    public void GenerateMapBtn()
    {
        var seed = seedInput.text;
        Player.localPlayer.GenerateMap(seed);
        startGameBtn.SetActive(true);
    }

    public void StartGameBtn()
    {
        Player.localPlayer.StartGame();
    }

    public void RollDice()
    {
        Player.localPlayer.RollDice();
        rollDiceBtn.GetComponent<Button>().interactable = false;

        if (GameManager.Instance.curGameState == Enums.GameState.inGame) finishTurnBtn.SetActive(true);
    }

    public void FinishTurnBtn()
    {
        Player.localPlayer.FinishTurn();
    }

    // -- UI Updates per State
    public void SwitchGameStateUI(Enums.GameState newState)
    {
        Debug.Log("Switch GameState UI");
        switch (newState)
        {
            case Enums.GameState.mapGeneration:
                ActivateMapGenerationUIHost(true);
                break;
            case Enums.GameState.turnDetermization:
                ActivateMapGenerationUI(false);
                ActivateInfoTexts(true);
                break;
            case Enums.GameState.preGame:
                if (Player.localPlayer == TurnManager.Instance.curPlayer)
                {
                    clickManager.GetComponent<ObjectClicker>().enabled = true;
                }
                ActivateDiceUI(false);
                ActivateRemoteDiceUI(false);
                break;
            case Enums.GameState.inGame:
                // Activate Resource UI, etc..

                if (Player.localPlayer.isCurPlayer)
                {
                    ActivateCurPlayerUI();
                }
                else
                {
                    DeActivateCurPlayerUI();
                }

                break;
        }
    }

    void ActivateMapGenerationUIHost(bool activate)
    {
        if (Player.localPlayer.isServer)
        {
            mapGenerationStateBtns.SetActive(activate);
            seedInput.interactable = activate;
        }
    }

    void ActivateMapGenerationUI(bool activate)
    {
        mapGenerationStateBtns.SetActive(activate);
        seedInputParent.SetActive(activate);
    }

    void ActivateInfoTexts(bool activate)
    {
        curPlayerParent.SetActive(activate);
    }

    // -- UI Updates per CurPlayer
    public void ActivateCurPlayerUI()
    {
        Debug.Log("Activate CurPlayer UI");

        if (GameManager.Instance.curGameState == Enums.GameState.inGame)
        {
            //finishTurnBtn.SetActive(true);
        }

        if (GameManager.Instance.curGameState != Enums.GameState.preGame)
        {
            ActivateDiceUI(true);
            ActivateRemoteDiceUI(false);
        }

        if (GameManager.Instance.curGameState == Enums.GameState.inGame || GameManager.Instance.curGameState == Enums.GameState.preGame)
        {
            clickManager.GetComponent<ObjectClicker>().enabled = true;
        }
    }

    public void DeActivateCurPlayerUI()
    {
        Debug.Log("DeActivate CurPlayer UI");

        ActivateDiceUI(false);
        ActivateRemoteDiceUI(true);

        finishTurnBtn.SetActive(false);

        clickManager.GetComponent<ObjectClicker>().enabled = false;
    }

    void ActivateDiceUI(bool activate)
    {
        diceValLocal.text = "";
        diceValLocal.gameObject.SetActive(activate);
        rollDiceBtn.SetActive(activate);
        rollDiceBtn.GetComponent<Button>().interactable = activate;
    }

    void ActivateRemoteDiceUI(bool activate)
    {
        diceValRemote.text = "";
        diceValRemote.gameObject.SetActive(activate);
    }

    // -- Text Updates --
    public void UpdateSeedVal(int seed)
    {
        seedInput.text = seed.ToString(); 
    }

    public void UpdateCurPlayerVal(string curPlayer)
    {
        curPlayerVal.text = curPlayer;
    }

    public void UpdateCurStateVal(string curState)
    {
        curStateVal.text = curState;
    }

    public void SetLocalPlayerName(string name)
    {
        localPlayerName.text = name;
    }

    // -- Dice --
    public void ShowDiceVal(int diceSum, int diceVal1, int diceVal2)
    {
        isDiceRolling = true;
        Debug.Log($"Show Dice Val {diceVal1} + {diceVal2} = {diceSum}");
        StartCoroutine(AnimateDice());

        StartCoroutine(WaitForDiceAnimation(diceSum, diceVal1, diceVal2));
    }

    private IEnumerator AnimateDice()
    {
        int i = 1;
        int j = 5;
        while (isDiceRolling)
        {
                diceValLocal.text = i + " + " + j + " = ?";
                diceValRemote.text = i + " + " + j + " = ?";

            i++; j++;
            if (i == 7) i = 1;
            if (j == 7) j = 1;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private IEnumerator WaitForDiceAnimation(int diceSum, int diceVal1, int diceVal2)
    {
        yield return new WaitForSeconds(1);

        isDiceRolling = false;
        
        diceValLocal.text = diceVal1 + " + " + diceVal2 + " = " + diceSum;
        diceValRemote.text = diceVal1 + " + " + diceVal2 + " = " + diceSum;
    }

    // -- Place Building --
    public void OpenPlacementConfirmation()
    {
        placementConfirmationMenu.SetActive(true);
    }

    public void PlacementConfirmationYes()
    {
        Player.localPlayer.ConfirmPlacement(true);
        placementConfirmationMenu.SetActive(false);
    }

    public void PlacementConfirmationNo()
    {
        Player.localPlayer.ConfirmPlacement(false);
        placementConfirmationMenu.SetActive(false);
    }

    // -- Upgrade Building --
    public void OpenUpgradingConfirmation()
    {
        upgradeConfirmationMenu.SetActive(true);
    }

    public void UpgradingConfirmationYes()
    {
        Player.localPlayer.ConfirmUpgrade(true);
        upgradeConfirmationMenu.SetActive(false);
    }

    public void UpgradingConfirmationNo()
    {
        Player.localPlayer.ConfirmUpgrade(false);
        upgradeConfirmationMenu.SetActive(false);
    }

    #region Trade UI
    public void TradePanel()
    {
        if (tradePanel.activeSelf)
        {
            tradePanel.SetActive(false);
        }
        else
        {
            tradePanel.SetActive(true);
        }
    }
    #endregion
}
