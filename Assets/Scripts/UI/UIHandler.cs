using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIHandler : MonoBehaviour
{
    public GameObject generateMapBtn;
    public InputField seedInput;

    public void GenerateMapBtn()
    {
        var seed = seedInput.text;
        Player.localPlayer.GenerateMap(seed);
    }

    public void ActivateUI()
    {
        generateMapBtn.SetActive(true);
        seedInput.interactable = true;
    }

    public void ShowSeed(int seed)
    {
        seedInput.text = seed.ToString(); 
    }

}
