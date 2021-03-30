using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberInput : MonoBehaviour
{
    public GameObject inputField;
    public CameraMovement cameraComponent;

	void Update()
	{
        inputField = GameObject.Find("/-UI/Canvas/UI_TextInput/NumberInput");
        cameraComponent = Camera.main.GetComponent<CameraMovement>();
		//Debug.Log(inputField);
  //      Debug.Log(inputField.GetComponent<InputField>());
  //      Debug.Log(inputField.GetComponent<InputField>().isFocused);
        if(inputField.GetComponent<InputField>().isFocused == true) {
            cameraComponent.setMovable(false);
        } else {
            cameraComponent.setMovable(true);
        }
	}
}
