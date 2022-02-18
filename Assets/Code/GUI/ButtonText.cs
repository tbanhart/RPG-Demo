using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonText : MonoBehaviour
{
    [SerializeField] GameObject buttonText;

    public void SetText(string text){
        buttonText.GetComponent<Text>().text = text;
    }
}
