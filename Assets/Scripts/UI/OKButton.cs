using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKButton : MonoBehaviour
{
    public Button button;

    void Start () {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(GetInput);
    }

    public void GetInput() {
        GameObject inputField = GameObject.Find("/-UI/Canvas/UI_TextInput/NumberInput");
        InputField inputFieldCo = inputField.GetComponent<InputField>();
        Debug.Log(inputFieldCo.text);
    }
}
