using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExamineText : MonoBehaviour
{
    [SerializeField] GameObject TextObject;
    TextMeshProUGUI displaytext;

    private void Awake() {
        displaytext = TextObject.GetComponent<TextMeshProUGUI>();
    }

    internal void SetText(string text){
        displaytext.text = text;
    }
    
    internal void ClearText(){
        displaytext.text = string.Empty;
    }
}
