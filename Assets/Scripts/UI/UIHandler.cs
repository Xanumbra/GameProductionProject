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

    // -- UI Updates per State
    public void SwitchGameStateUI(Enums.GameState newState)
    {
        Debug.Log("ui handler switch");

        switch (newState)
        {
            case Enums.GameState.mapGeneration:
                ActivateMapGenerationUIHost(true);
                break;
            case Enums.GameState.turnDetermization:
                Debug.Log("ui handler switch case turn detem");
                ActivateMapGenerationUI(false);
                ActivateInfoTexts(true);
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

}
