using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offer : MonoBehaviour
{

    public bool isTradingWithBank;
    public int darkMatterOffer;
    public int spacePigOffer;
    public int waterOffer;
    public int metalOffer;
    public int energyOffer;

    public Offer(int darkMatterAmount,int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        darkMatterOffer = darkMatterAmount;
        spacePigOffer = spacePigAmount;
        waterOffer = waterAmount;
        metalOffer = metalAmount;
        energyOffer = energyAmount;
    }
    public void TradeWithBank()
    {
        isTradingWithBank = true;
    }
    public void TradeWithPlayers()
    {
        isTradingWithBank = false;
    }
}
