using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TradeResourceButton : MonoBehaviour
{
    public Enums.Resources resourceType;
    public int adjustAmount;

    public void AdjustResource()
    {
        TradeManager.Instance.ChangeResourceAmount(resourceType, adjustAmount);
    }


}
