using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Text darkMatterAmountText;
    public Text spacePigAmountText;
    public Text waterAmountText;
    public Text metalAmountText;
    public Text energyAmountText;

    public GameObject tradePopupPrefab;

    public TradeStatusButton player1StatusButton;
    public TradeStatusButton player2StatusButton;
    public TradeStatusButton player3StatusButton;

    public int possibleTraders;

    public Text tradePopUpOfferOwner;
    public Text tradePopUpOfferInfo;
    public Offer offer;

    public bool isTradingWithBank = true;
    public bool isTrading = false;

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
        if (isTradingWithBank)
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
    public void ChangeResourceText(Text resourceText,int resourceAmount)
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
        spacePigAmount = 0;
        waterAmount = 0;
        metalAmount = 0;
        energyAmount = 0;
    }
    public void CreateTradeOffer()
    {
        if (CheckTradeValidityForPlayerTrade() && !isTradingWithBank && !isTrading && isOfferContainingResource())
        { 
            SendTradeOffer(Player.localPlayer.clientId, darkMatterAmount, spacePigAmount, waterAmount, metalAmount, energyAmount);
            isTrading = true;
        }
        else if (CheckTradeValidityForBankTrade() && isTradingWithBank && !isTrading && isOfferContainingResource())
        {
            Player.localPlayer.darkMatterAmount += darkMatterAmount;
            Player.localPlayer.spacePigAmount += spacePigAmount;
            Player.localPlayer.waterAmount += waterAmount;
            Player.localPlayer.metalAmount += metalAmount;
            Player.localPlayer.energyAmount += energyAmount;
            ResetResourceAmount();
        }
    }
    [Command]
    public void SendTradeOffer(int ownerId, int darkMatterAmount, int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        ShowTradeOffer(ownerId,darkMatterAmount, spacePigAmount, waterAmount, metalAmount, energyAmount);
    }
    [ClientRpc]
    public void ShowTradeOffer(int ownerId, int darkMatterAmount, int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        for (int i = 0; i < possibleTraders; i++)
        {
            if (i != ownerId)
            {
                DisplayOffer(ownerId,darkMatterAmount,spacePigAmount, waterAmount, metalAmount, energyAmount);
            }
        }
    }
    public bool CheckTradeValidityForPlayerTrade()
    {
        if(Player.localPlayer.darkMatterAmount + darkMatterAmount >= 0
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
    public void DisplayOffer(int tradeOwnerId, int darkMatterAmount, int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        tradePopUpOfferOwner.text = "";
        tradePopUpOfferInfo.text = "";
        tradePopUpOfferOwner.text += "Trade Offer from\nPlayer " + tradeOwnerId;
        tradePopUpOfferInfo.text += darkMatterAmount != 0 ? "\nDarkMatter: " + this.darkMatterAmount : "";
        tradePopUpOfferInfo.text += spacePigAmount != 0 ? "\nSpacePig: " + this.spacePigAmount : "";
        tradePopUpOfferInfo.text += waterAmount != 0 ? "\nWater: " + this.waterAmount : "";
        tradePopUpOfferInfo.text += metalAmount != 0 ? "\nMetal: " + this.metalAmount : "";
        tradePopUpOfferInfo.text += energyAmount != 0 ? "\nEnergy: " + this.energyAmount : "";
        offer = new Offer(darkMatterAmount, spacePigAmount, waterAmount, metalAmount, energyAmount);
        offer.ownerId = tradeOwnerId;
        tradePopupPrefab.SetActive(true);
        // Make a change on TradeUI so the owner can see the differences
    }

    public void AcceptOffer()
    {
        if (Player.localPlayer.darkMatterAmount >= offer.darkMatterOffer
            && Player.localPlayer.spacePigAmount >= offer.spacePigOffer
            && Player.localPlayer.waterAmount >= offer.waterOffer
            && Player.localPlayer.metalAmount >= offer.metalOffer
            && Player.localPlayer.energyAmount >= offer.energyOffer)
        {
            NetworkIdentity target = TurnManager.Instance.players.Where(p => p.clientId == offer.ownerId).Select(p => p).First().GetComponent<NetworkIdentity>();
            DisplayPlayerStatus(target.connectionToClient, true, Player.localPlayer.clientId);
        }
        
    }
    public void DeclineOffer()
    {
        NetworkIdentity target = TurnManager.Instance.players.Where(p => p.clientId == offer.ownerId).Select(p => p).First().GetComponent<NetworkIdentity>();
        DisplayPlayerStatus(target.connectionToClient, false, Player.localPlayer.clientId);
    }
    [TargetRpc]
    public void DisplayPlayerStatus(NetworkConnection target,bool acceptedTrade,int playerId)
    {
        int targetStatusButton = FindStatusButtonForPlayer(Player.localPlayer.clientId, playerId);
        if(targetStatusButton == 1)
        {
            player1StatusButton.ChangeStatusImage(acceptedTrade);
        }else if(targetStatusButton == 2)
        {
            player2StatusButton.ChangeStatusImage(acceptedTrade);
        }else if(targetStatusButton == 3)
        {
            player3StatusButton.ChangeStatusImage(acceptedTrade);
        }
    }
    public void CompleteTrade()
    {
        if (isTrading)
        {
            if (player1StatusButton.isSelected)
            {
                ChangeResourceOnPlayer(Player.localPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    darkMatterAmount,
                    spacePigAmount,
                    waterAmount,
                    metalAmount,
                    energyAmount);
                ChangeResourceOnPlayer(player1StatusButton.referencedPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    -darkMatterAmount,
                    -spacePigAmount,
                    -waterAmount,
                   -metalAmount,
                    -energyAmount);
                isTrading = false;
                ResetResourceAmount();
                player1StatusButton.ResetTradeStatusButton();
                player2StatusButton.ResetTradeStatusButton();
                player3StatusButton.ResetTradeStatusButton();
            }
            else if (player2StatusButton.isSelected)
            {
                ChangeResourceOnPlayer(Player.localPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    darkMatterAmount,
                    spacePigAmount,
                    waterAmount,
                    metalAmount,
                    energyAmount);
                ChangeResourceOnPlayer(player2StatusButton.referencedPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    -darkMatterAmount,
                    -spacePigAmount,
                    -waterAmount,
                    -metalAmount,
                    -energyAmount);
                isTrading = false;
                ResetResourceAmount();
                player1StatusButton.ResetTradeStatusButton();
                player2StatusButton.ResetTradeStatusButton();
                player3StatusButton.ResetTradeStatusButton();
            }
            else if (player3StatusButton.isSelected)
            {
                ChangeResourceOnPlayer(Player.localPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    darkMatterAmount,
                    spacePigAmount,
                    waterAmount,
                    metalAmount,
                    energyAmount);
                ChangeResourceOnPlayer(player3StatusButton.referencedPlayer.GetComponent<NetworkIdentity>().connectionToClient,
                    -darkMatterAmount,
                    -spacePigAmount,
                    -waterAmount,
                    -metalAmount,
                    -energyAmount);
                isTrading = false;
                ResetResourceAmount();
                player1StatusButton.ResetTradeStatusButton();
                player2StatusButton.ResetTradeStatusButton();
                player3StatusButton.ResetTradeStatusButton();
            }
        }

    }
    [TargetRpc]
    public void ChangeResourceOnPlayer(NetworkConnection target,int darkMatterAmount,int spacePigAmount, int waterAmount, int metalAmount, int energyAmount)
    {
        Player.localPlayer.darkMatterAmount += darkMatterAmount;
        Player.localPlayer.spacePigAmount += spacePigAmount;
        Player.localPlayer.waterAmount += waterAmount;
        Player.localPlayer.metalAmount += metalAmount;
        Player.localPlayer.energyAmount += energyAmount;
    }
    public int FindStatusButtonForPlayer(int tradeOwnerId, int playerId)
    {
        if(tradeOwnerId == 0)
        {
            return playerId;
        }else if(tradeOwnerId == 1)
        {
            if(playerId == 0)
            {
                return 1;
            }
            else
            {
                return playerId - 1;
            }
        }else if(tradeOwnerId == 2)
        {
            if(playerId != 3)
            {
                return playerId + 1;
            }
            else
            {
                return playerId;
            }
        }else if(tradeOwnerId == 3)
        {
            return playerId + 1;
        }
        else
        {
            return -1;
        }
    }
    [ClientRpc]
    public void SetStatusButtonReferences()
    {
        List<Player> otherPlayers = TurnManager.Instance.players.Where(p => p.clientId != Player.localPlayer.clientId).Select(p => p).ToList();
        if(possibleTraders == 1)
        {
            player1StatusButton.referencedPlayer = otherPlayers[0];
        }else if(possibleTraders == 2)
        {
            player1StatusButton.referencedPlayer = otherPlayers[0];
            player2StatusButton.referencedPlayer = otherPlayers[1];
        }
        else if(possibleTraders == 3)
        {
            player1StatusButton.referencedPlayer = otherPlayers[0];
            player2StatusButton.referencedPlayer = otherPlayers[1];
            player3StatusButton.referencedPlayer = otherPlayers[2];
        }
    }
    public void SwitchToBankTrade()
    {
        isTradingWithBank = true;
    }
    public void SwitchToPlayerTrade()
    {
        isTradingWithBank = false;
    }
}
