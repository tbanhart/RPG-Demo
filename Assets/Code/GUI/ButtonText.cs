using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonText : MonoBehaviour
{
    [SerializeField] GameObject buttonText;

    private void Awake() {
    }

    public void SetText(string text){
        buttonText.GetComponent<Text>().text = text;
    }
}
