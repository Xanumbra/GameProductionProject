using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] TMP_InputField ipInput;

    public void HostGame()
    {
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        var ip = ipInput.text;
        if (ip.Length < 7)
        {
            Debug.Log("Invalid IP Adress");
        }
        else
        {
            try
            {
                networkManager.networkAddress = ip;
                networkManager.StartClient();
            }
            catch
            {
                Debug.Log("Host not found");
            }
        }
    }
}
