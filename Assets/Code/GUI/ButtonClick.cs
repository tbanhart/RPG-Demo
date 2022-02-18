using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClick : MonoBehaviour
{
    UnityEvent click;

    private void Start() {
        if(click == null){
            click = new UnityEvent();

            click.AddListener(OnClick);
        }
    }

    public void OnClick(){
        Debug.Log("Button CLicked");
    }
}
