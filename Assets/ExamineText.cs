using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineText : MonoBehaviour
{
    [SerializeField] GameObject TextObject;
    Text displaytext;

    private void Awake() {
        displaytext = TextObject.GetComponent<Text>();
    }

    internal void SetText(string text){
        displaytext.text = text;
    }
    
    internal void ClearText(){
        displaytext.text = string.Empty;
    }
}
