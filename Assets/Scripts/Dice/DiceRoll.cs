using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DiceRoll : NetworkBehaviour
{
    public static int diceOne;
    public static int diceTwo;
    public static int RollDiceStatic()
    {
        diceOne = Random.Range(1, 7);
        diceTwo = Random.Range(1, 7);
        return diceOne + diceTwo;
    }

    public int RollDice(out int diceVal1, out int diceVal2)
    {
        Random.InitState(System.Environment.TickCount);
        diceOne = Random.Range(1, 7);
        diceTwo = Random.Range(1, 7);
        diceVal1 = diceOne;
        diceVal2 = diceTwo;
        return diceOne + diceTwo;
    }
}
