using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offer : MonoBehaviour
{

    public Player OfferOwner;
    public int GivenDarkMatter;
    public int GivenSpacePig;
    public int GivenMetal;
    public int GivenEnergy;
    public int GivenWater;
    public int TakenDarkMatter;
    public int TakenSpacePig;
    public int TakenMetal;
    public int TakenEnergy;
    public int TakenWater;

    public Offer()
    {
        GivenDarkMatter = 0;
        GivenSpacePig = 0;
        GivenMetal = 0;
        GivenEnergy = 0;
        GivenWater = 0;
        TakenDarkMatter = 0;
        TakenSpacePig = 0;
        TakenMetal = 0;
        TakenEnergy = 0;
        TakenWater = 0;
    }
    public void SetGivenOfferResources(int GivenDarkMatter, int GivenSpacePig, int GivenMetal, int GivenEnergy, int GivenWater)
    {
        this.GivenDarkMatter = GivenDarkMatter;
        this.GivenSpacePig = GivenSpacePig;
        this.GivenMetal = GivenMetal;
        this.GivenEnergy = GivenEnergy;
        this.GivenWater = GivenWater;
    }
    public void SetTakenOfferResources(int TakenDarkMatter, int TakenSpacePig, int TakenMetal, int TakenEnergy, int TakenWater)
    {
        this.TakenDarkMatter = TakenDarkMatter;
        this.TakenSpacePig = TakenSpacePig;
        this.TakenMetal = TakenMetal;
        this.TakenEnergy = TakenEnergy;
        this.TakenWater = TakenWater;
    }
    public void SetOfferOwner(Player OfferOwner)
    {
        this.OfferOwner = OfferOwner;
    }
}
