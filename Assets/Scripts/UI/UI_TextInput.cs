using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextInput : MonoBehaviour
{

    // public void Awake() {
    //     Hide();
    // }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void GetInput() {
        GameObject inputField = GameObject.Find("/Canvas/UI_TextInput/NumberInput/Text");
        InputField inputFieldCo = inputField.GetComponent<InputField>();
        Debug.Log(inputFieldCo.text);
    }

    
}
