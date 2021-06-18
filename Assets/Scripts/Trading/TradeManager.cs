using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : NetworkBehaviour
{
    public int darkMatterAmount = 0;
    public int spacePigAmount = 0;
    public int waterAmount = 0;
    public int metalAmount = 0;
    public int energyAmount = 0;

    public int darkMatterHarborRatio = 4;
    public int spacePigHarborRatio = 4;
    public int waterHarborRatio = 4;
    public int metalHarborRatio = 4;
    public int energyHarborRatio = 4;

    public TMP_Text darkMatterAmountText;
    public TMP_Text spacePigAmountText;
    public TMP_Text waterAmountText;
    public TMP_Text metalAmountText;
    public TMP_Text energyAmountText;


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
        amount *= 4;
        switch(resourceType){
            case Enums.Resources.darkMatter:
                darkMatterAmount += amount;
                ChangeResourceText(darkMatterAmountText, darkMatterAmount);
                break;
            case Enums.Resources.spacePig:
                spacePigAmount += amount;
                ChangeResourceText(spacePigAmountText, spacePigAmount);
                break;
            case Enums.Resources.water:
                waterAmount += amount;
                ChangeResourceText(waterAmountText, waterAmount);
                break;
            case Enums.Resources.metal:
                metalAmount += amount;
                ChangeResourceText(metalAmountText, metalAmount);
                break;
            case Enums.Resources.energy:
                energyAmount += amount;
                ChangeResourceText(energyAmountText, energyAmount);
                break;
        }
    }
    public bool isOfferContainingResource()
    {
        if(darkMatterAmount == 0 && spacePigAmount == 0 && waterAmount == 0 && metalAmount == 0 && energyAmount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void ChangeResourceText(TMP_Text resourceText,int resourceAmount)
    {
        if(resourceAmount > 0)
        {
            resourceText.text = "+" + resourceAmount;
        }
        else if(resourceAmount < 0)
        {
            resourceText.text = resourceAmount.ToString();
        }
        else
        {
            resourceText.text = "0";
        }
    }
    public void ResetResourceAmount()
    {
        darkMatterAmount = 0;
        ChangeResourceText(darkMatterAmountText, 0);
        spacePigAmount = 0;
        ChangeResourceText(spacePigAmountText, 0);
        waterAmount = 0;
        ChangeResourceText(waterAmountText, 0);
        metalAmount = 0;
        ChangeResourceText(metalAmountText, 0);
        energyAmount = 0;
        ChangeResourceText(energyAmountText, 0);
    }
    public void CompleteTrade()
    {
        if (CheckTradeValidityForBankTrade())
        {
            if (isOfferContainingResource())
            {
                if (CheckTradeValidityForPlayerResources())
                {
                    Player.localPlayer.darkMatterAmount += darkMatterAmount;
                    Player.localPlayer.spacePigAmount += spacePigAmount;
                    Player.localPlayer.waterAmount += waterAmount;
                    Player.localPlayer.metalAmount += metalAmount;
                    Player.localPlayer.energyAmount += energyAmount;
                    ResetResourceAmount();
                }
                else
                {
                    InfoBoxManager.Instance.ClientWriteMessage("You don't have enough resources!");
                }
                
            }
            else
            {
                InfoBoxManager.Instance.ClientWriteMessage("Your offer does not contain resources!");
            }

        }
        else
        {
            InfoBoxManager.Instance.ClientWriteMessage("Offer is not valid!");
        }
        
    }
    public bool CheckTradeValidityForPlayerResources()
    {
        if (Player.localPlayer.darkMatterAmount + darkMatterAmount >= 0
                && Player.localPlayer.spacePigAmount + spacePigAmount >= 0
                && Player.localPlayer.waterAmount + waterAmount >= 0
                && Player.localPlayer.metalAmount + metalAmount >= 0
                && Player.localPlayer.energyAmount + energyAmount >= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckTradeValidityForBankTrade()
    {
        bool sumHasChanged = false;
        int sum = 0;
        if(darkMatterAmount < 0 && darkMatterAmount % darkMatterHarborRatio == 0)
        {
            sum += darkMatterAmount / darkMatterHarborRatio;
            sumHasChanged = true;
        }else if(darkMatterAmount > 0)
        {
            sum += darkMatterAmount;
            sumHasChanged = true;
        }
        if(spacePigAmount < 0 && spacePigAmount % spacePigHarborRatio == 0)
        {
            sum += spacePigAmount / spacePigHarborRatio;
            sumHasChanged = true;
        }
        else if(spacePigAmount > 0)
        {
            sum += spacePigAmount;
            sumHasChanged = true;
        }
        if(waterAmount < 0 && waterAmount % waterHarborRatio == 0)
        {
            sum += waterAmount / waterHarborRatio;
            sumHasChanged = true;
        }
        else if(waterAmount > 0)
        {
            sum += waterAmount;
            sumHasChanged = true;
        }
        if(metalAmount < 0 && metalAmount % metalHarborRatio == 0)
        {
            sum += metalAmount / metalHarborRatio;
            sumHasChanged = true;
        }
        else if(metalAmount > 0)
        {
            sum += metalAmount;
            sumHasChanged = true;
        }
        if(energyAmount < 0 && energyAmount % energyHarborRatio == 0)
        {
            sum += energyAmount / energyHarborRatio;
            sumHasChanged = true;
        }
        else if(energyAmount > 0)
        {
            sum += energyAmount;
            sumHasChanged = true;
        }

        if(sum == 0 && sumHasChanged)
        {
            return true;
        }
        else
        {
            return false;
        }
}
}
