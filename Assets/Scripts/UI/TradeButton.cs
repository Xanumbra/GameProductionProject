using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{
    bool clicked = false;
    public Button yourButton;
    public GameObject target;

    public Button tradeCloseButton;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        target.SetActive(false);

        Button btnTradeCloseButton = tradeCloseButton.GetComponent<Button>();
        btnTradeCloseButton.onClick.AddListener(CloseOnClick);
    }

    // Update is called once per frame
    void TaskOnClick(){
        target.SetActive(true);
    }

    void CloseOnClick()
    {
        Debug.Log ("You have clicked the button!");
        target.SetActive(false);
    }
}
