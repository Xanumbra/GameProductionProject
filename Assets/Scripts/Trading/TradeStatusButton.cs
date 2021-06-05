using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeStatusButton : MonoBehaviour
{
    public Player referencedPlayer;
    public Sprite checkSprite;
    public Sprite crossSprite;
    public GameObject statusImageObject;
    public bool isSelected = false;


    public void ChangeStatusImage(bool acceptedTrade)
    {
        if (acceptedTrade)
        {
            statusImageObject.GetComponent<Image>().sprite = checkSprite;
        }
        else
        {
            statusImageObject.GetComponent<Image>().sprite = crossSprite;
        }
        statusImageObject.SetActive(true);
    }
    public void ResetTradeStatusButton()
    {
        statusImageObject.SetActive(false);
        isSelected = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }
    }


}
