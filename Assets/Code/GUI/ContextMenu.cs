using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    List<GameObject> Buttons;

    [SerializeField] GameObject Button;

    [SerializeField] GameObject ActionsParent;

    public ContextMenu(){
        Buttons = new List<GameObject>();
    }

    private void Awake() {
        ClearMenu();
    }

    public void AddAction(ActionType action){
        var button = Instantiate(Button);
        button.transform.parent = ActionsParent.transform;
        Buttons.Add(button);
        button.GetComponent<ButtonText>().SetText($"{action}");
    }

    public void ClearMenu(){
        foreach (var button in Buttons)
        {
            Destroy(button);
        }
        Buttons.Clear();
    }

    public void Close(){
        ClearMenu();
    }
}
