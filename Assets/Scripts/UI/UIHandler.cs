using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject generateMapBtn;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void ActivateGenerateMapBtn()
    {
        generateMapBtn.SetActive(true);
    }

    public void GenerateMapBtn()
    {
        Player.localPlayer.GenerateMap();
    }
}
