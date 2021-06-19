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
            TradeManager.Instance.ChangeResourceAmount(resourceType, adjustAmount);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            TradeManager.Instance.ChangeResourceAmount(resourceType, adjustAmount * 4);
        }
    }
}
