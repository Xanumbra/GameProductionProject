using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public static int diceOne;
    public static int diceTwo;
    public static int RollDice()
    {
        diceOne = Random.Range(1, 7);
        diceTwo = Random.Range(1, 7);
        return diceOne + diceTwo;
    }
}
