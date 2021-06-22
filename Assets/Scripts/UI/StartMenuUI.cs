using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] GameObject creditsOverlay;
    [SerializeField] GameObject creditsButton;

    public void Credits()
    {
        creditsOverlay.SetActive(!creditsOverlay.activeInHierarchy);

        if (creditsOverlay.activeInHierarchy)
        {
            creditsButton.GetComponentInChildren<TMP_Text>().text = "Close";
        }
        else
        {
            creditsButton.GetComponentInChildren<TMP_Text>().text = "Credits";
        }
    }

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
