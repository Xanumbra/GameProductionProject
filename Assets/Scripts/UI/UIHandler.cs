using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIHandler : MonoBehaviour
{
    public GameObject mapGenerationStateBtns;
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

    private void Start()
    {
        seedInput = seedInputParent.GetComponentInChildren<InputField>();
    }

    // -- Button Methods --
    public void GenerateMapBtn()
    {
        var seed = seedInput.text;
        Player.localPlayer.GenerateMap(seed);
    }

    public void StartGameBtn()
    {
        Player.localPlayer.StartGame();
    }

    public void RollDice()
    {
        Player.localPlayer.RollDice();
        rollDiceBtn.GetComponent<Button>().interactable = false;
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
    public void ActivateCurPlayerUI(Player curPlayer)
    {
        Debug.Log("Activate CurPlayer UI");

        ActivateDiceUI(true);
        ActivateRemoteUI(false);

        finishTurnBtn.SetActive(true);

        clickManager.SetActive(true);
    }

    public void DeActivatecurPlayerUI()
    {
        Debug.Log("DeActivate CurPlayer UI");

        ActivateDiceUI(false);
        ActivateRemoteUI(true);

        finishTurnBtn.SetActive(false);

        clickManager.SetActive(false);
    }

    void ActivateDiceUI(bool activate)
    {
        diceValLocal.text = "";
        diceValLocal.gameObject.SetActive(activate);
        rollDiceBtn.SetActive(activate);
        rollDiceBtn.GetComponent<Button>().interactable = activate;
    }

    void ActivateRemoteUI(bool activate)
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
        yield return new WaitForSeconds(2);

        isDiceRolling = false;
        
        diceValLocal.text = diceVal1 + " + " + diceVal2 + " = " + diceSum;
        diceValRemote.text = diceVal1 + " + " + diceVal2 + " = " + diceSum;
    }
}
