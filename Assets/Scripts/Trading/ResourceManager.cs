using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : NetworkBehaviour
{
    public TMP_Text darkMatterText;
    public TMP_Text spacePigText;
    public TMP_Text metalText;
    public TMP_Text waterText;
    public TMP_Text energyText;

    private static ResourceManager _instance;

    public static ResourceManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    [ClientRpc]
    public void UpdateResourceUI()
    {
        darkMatterText.text = Player.localPlayer.darkMatterAmount.ToString();
        spacePigText.text = Player.localPlayer.spacePigAmount.ToString();
        metalText.text = Player.localPlayer.metalAmount.ToString();
        waterText.text = Player.localPlayer.waterAmount.ToString();
        energyText.text = Player.localPlayer.energyAmount.ToString();
    }
}
