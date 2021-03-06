using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour
{
    public Button button;

    void Start () {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(RemoveInput);
    }

    public void RemoveInput() {
        GameObject inputField = GameObject.Find("/-UI/Canvas/UI_TextInput/NumberInput");
        InputField inputFieldCo = inputField.GetComponent<InputField>();
        inputFieldCo.text = "";
    }
}
