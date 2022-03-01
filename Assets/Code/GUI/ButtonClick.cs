using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClick : MonoBehaviour
{
    UnityEvent click;

    GameObject PlayerExecuting;

    private void Start() {
        if(click == null){
            click = new UnityEvent();

            click.AddListener(OnClick);
        }
    }

    public void InteractionClick(int id){
        
    }

    public void OnClick(){
        Debug.Log("Button CLicked");
    }
}
