using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : NetworkBehaviour
{
    public int darkMatterAmount = 0;
    public int spacePigAmount = 0;
    public int waterAmount = 0;
    public int metalAmount = 0;
    public int energyAmount = 0;

    public Text darkMatterAmountText;
    public Text SpacePigAmountText;
    public Text WaterAmountText;
    public Text MetalAmountText;
    public Text EnergyAmountText;

    public GameObject tradePopupPrefab;
    public Text tradePopUpOfferOwner;
    public Text tradePopUpOfferInfo;

    private static TradeManager _instance;

    public static TradeManager Instance { get { return _instance; } }
    void Awake()
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
    public void ChangeResourceAmount(Enums.Resources resourceType,int amount)
    {
        switch(resourceType){
            case Enums.Resources.darkMatter:
                darkMatterAmount += amount;
                if(darkMatterAmount > 0)
                {
                    darkMatterAmountText.text = "+" + darkMatterAmount;
                }else if(darkMatterAmount < 0)
                {
                    darkMatterAmountText.text = darkMatterAmount.ToString();
                }
                else
                {
                    darkMatterAmountText.text = "0";
                }
                break;
            case Enums.Resources.spacePig:
                spacePigAmount += amount;
                if (spacePigAmount > 0)
                {
                    SpacePigAmountText.text = "+" + spacePigAmount;
                }
                else if (spacePigAmount < 0)
                {
                    SpacePigAmountText.text = spacePigAmount.ToString();
                }
                else
                {
                    SpacePigAmountText.text = "0";
                }
                break;
            case Enums.Resources.water:
                waterAmount += amount;
                if (waterAmount > 0)
                {
                    WaterAmountText.text = "+" + waterAmount;
                }
                else if (waterAmount < 0)
                {
                    WaterAmountText.text = waterAmount.ToString();
                }
                else
                {
                    WaterAmountText.text = "0";
                }
                break;
            case Enums.Resources.metal:
                metalAmount += amount;
                if (metalAmount > 0)
                {
                    MetalAmountText.text = "+" + metalAmount;
                }
                else if (metalAmount < 0)
                {
                    MetalAmountText.text = metalAmount.ToString();
                }
                else
                {
                    MetalAmountText.text = "0";
                }
                break;
            case Enums.Resources.energy:
                energyAmount += amount;
                if (energyAmount > 0)
                {
                    EnergyAmountText.text = "+" + energyAmount;
                }
                else if (energyAmount < 0)
                {
                    EnergyAmountText.text = energyAmount.ToString();
                }
                else
                {
                    EnergyAmountText.text = "0";
                }
                break;
        }
    }
    public void ResetResourceAmount()
    {
        darkMatterAmount = 0;
        spacePigAmount = 0;
        waterAmount = 0;
        metalAmount = 0;
        energyAmount = 0;
    }

    
    public void CreateTradeOffer()
    {
        if (CheckTradeValidityForPlayer())
        {
            Offer offer = new Offer(darkMatterAmount,spacePigAmount,waterAmount,metalAmount,energyAmount);
        }
        else
        {
            // Error about trade not being valid
        }
    }
    [Command]
    public void SendTradeOffer(int ownerId, Offer offer)
    {
        for(int i = 0; i < TurnManager.Instance.players.Count; i++)
        {
            if(i != ownerId)
            {
                ShowTradeOffer(i,ownerId, offer);
            }
        }
    }
    [TargetRpc]
    public void ShowTradeOffer(int targetClientId, int ownerId, Offer offer)
    {
        
    }
    public bool CheckTradeValidityForPlayer()
    {
        if(Player.localPlayer.darkMatterAmount < darkMatterAmount
            || Player.localPlayer.spacePigAmount < spacePigAmount
            || Player.localPlayer.waterAmount < waterAmount
            || Player.localPlayer.metalAmount < metalAmount
            || Player.localPlayer.energyAmount < energyAmount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void DisplayOffer(int tradeOwnerId,Offer offer)
    {
        tradePopUpOfferOwner.text = "";
        tradePopUpOfferInfo.text = "";
        tradePopUpOfferOwner.text += "Trade Offer from\nPlayer " + tradeOwnerId;
        tradePopUpOfferInfo.text += offer.darkMatterOffer != 0 ? "DarkMatter: " + darkMatterAmount : "";
        tradePopUpOfferInfo.text += offer.spacePigOffer != 0 ? "\nSpacePig: " + spacePigAmount : "";
        tradePopUpOfferInfo.text += offer.waterOffer != 0 ? "\nWater: " + waterAmount : "";
        tradePopUpOfferInfo.text += offer.metalOffer != 0 ? "\nMetal: " + metalAmount : "";
        tradePopUpOfferInfo.text += offer.energyOffer != 0 ? "\nEnergy: " + energyAmount : "";
        tradePopupPrefab.SetActive(true);
    }
}
