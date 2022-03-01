using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    List<GameObject> Buttons;

    [SerializeField] GameObject Button;

    [SerializeField] GameObject ActionsUIPanel;

    GridLayoutGroup gridSettings;

    public GameObject Target {get; set;}

    public ContextMenu(){
        Buttons = new List<GameObject>();
    }

    private void Awake() {
        gridSettings = ActionsUIPanel.GetComponent<GridLayoutGroup>();
        var sellsize = gridSettings.cellSize;
        ActionsUIPanel.GetComponent<GridLayoutGroup>().cellSize.Set(300, sellsize.y);
    }

    public void AddAction(GameObject player, ActionType action){
        var button = Instantiate(Button);
        button.transform.SetParent(ActionsUIPanel.transform);
        Buttons.Add(button);
        button.GetComponent<ButtonText>().SetupButton(player, $"{action}", action, Target);
        button.GetComponent<ButtonText>().ID = Buttons.FindIndex(o => o == button);
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
