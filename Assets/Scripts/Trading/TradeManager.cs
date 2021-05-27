using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    public int DarkMatterAmount = 0;
    public int SpacePigAmount = 0;
    public int WaterAmount = 0;
    public int MetalAmount = 0;
    public int EnergyAmount = 0;

    public Text darkMatterAmountText;
    public Text SpacePigAmountText;
    public Text WaterAmountText;
    public Text MetalAmountText;
    public Text EnergyAmountText;

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
                DarkMatterAmount += amount;
                if(DarkMatterAmount > 0)
                {
                    darkMatterAmountText.text = "+" + DarkMatterAmount;
                }else if(DarkMatterAmount < 0)
                {
                    darkMatterAmountText.text = DarkMatterAmount.ToString();
                }
                else
                {
                    darkMatterAmountText.text = "0";
                }
                break;
            case Enums.Resources.spacePig:
                SpacePigAmount += amount;
                if (SpacePigAmount > 0)
                {
                    SpacePigAmountText.text = "+" + SpacePigAmount;
                }
                else if (SpacePigAmount < 0)
                {
                    SpacePigAmountText.text = SpacePigAmount.ToString();
                }
                else
                {
                    SpacePigAmountText.text = "0";
                }
                break;
            case Enums.Resources.water:
                WaterAmount += amount;
                if (WaterAmount > 0)
                {
                    WaterAmountText.text = "+" + WaterAmount;
                }
                else if (WaterAmount < 0)
                {
                    WaterAmountText.text = WaterAmount.ToString();
                }
                else
                {
                    WaterAmountText.text = "0";
                }
                break;
            case Enums.Resources.metal:
                MetalAmount += amount;
                if (MetalAmount > 0)
                {
                    MetalAmountText.text = "+" + MetalAmount;
                }
                else if (MetalAmount < 0)
                {
                    MetalAmountText.text = MetalAmount.ToString();
                }
                else
                {
                    MetalAmountText.text = "0";
                }
                break;
            case Enums.Resources.energy:
                EnergyAmount += amount;
                if (EnergyAmount > 0)
                {
                    EnergyAmountText.text = "+" + EnergyAmount;
                }
                else if (EnergyAmount < 0)
                {
                    EnergyAmountText.text = EnergyAmount.ToString();
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
        DarkMatterAmount = 0;
        SpacePigAmount = 0;
        WaterAmount = 0;
        MetalAmount = 0;
        EnergyAmount = 0;
    }

}
