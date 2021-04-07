using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
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

    [SyncVar] public Enums.GameState curGameState;

    void Start()
    {
        curGameState = Enums.GameState.waitingForPlayers;
    }

    void Update()
    {
        
    }


}
