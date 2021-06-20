using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeResourceButton : MonoBehaviour, IPointerClickHandler
{
    public Enums.Resources resourceType;
    public int adjustAmount;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            TradeManager.Instance.ChangeOfferResourceAmount(resourceType, adjustAmount);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            TradeManager.Instance.ChangeOfferResourceAmount(resourceType, adjustAmount * 4);
        }
    }
}
