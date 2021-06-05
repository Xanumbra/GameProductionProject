using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Offer : MonoBehaviour
{
    [SerializeField] public int darkMatterOffer;
    [SerializeField] public int spacePigOffer;
    [SerializeField] public int waterOffer;
    [SerializeField] public int metalOffer;
    [SerializeField] public int energyOffer;
    [SerializeField] public int ownerId;
    

    public Offer(int darkMatterAmount,int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        darkMatterOffer = darkMatterAmount;
        spacePigOffer = spacePigAmount;
        waterOffer = waterAmount;
        metalOffer = metalAmount;
        energyOffer = energyAmount;
    }
}
