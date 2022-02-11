using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] GameObject contextMenuPanel;

    ContextMenu contextMenu {get => this.GetComponent<ContextMenu>();}

    public void OpenContextMenu(Vector3 point, GameObject obj)
    {
        contextMenuPanel.SetActive(true);
        contextMenu.ClearMenu();
        contextMenuPanel.GetComponent<RectTransform>().position = point;
        foreach(var action in obj.GetComponent<Interactable>().AvailableActions){
            contextMenu.AddAction(action);
        }
    }

    public void CloseContextMenu(){
        if(contextMenuPanel.activeSelf == true){
            contextMenu.GetComponent<ContextMenu>().Close();
            contextMenuPanel.SetActive(false);
        }
    }
}
