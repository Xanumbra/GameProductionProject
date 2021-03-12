using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkDebugger : NetworkBehaviour
{
    public void GetCurPlayerBtn()
    {
        Player.localPlayer.GetCurPlayer();
    }



}
