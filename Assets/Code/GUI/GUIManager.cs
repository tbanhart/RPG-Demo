using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    #region Reference Objects

    [SerializeField] GameObject contextMenuPanel;

    ContextMenu contextMenu;

    [SerializeField] GameObject examineTextPanel;

    ExamineText examineText;

    #endregion

    #region Unity Functions

    private void Awake() {
        contextMenu = GetComponent<ContextMenu>();
        examineText = GetComponent<ExamineText>();
        CloseMenus();
    }

    #endregion

    #region Generic Functions

    public void CloseMenus()
    {
        CloseContextMenu();
        CloseExamineText();
    }

    #endregion

    #region Context Menu

    public void OpenContextMenu(GameObject player, Vector3 point, GameObject obj)
    {
        contextMenuPanel.SetActive(true);
        contextMenu.ClearMenu();
        contextMenuPanel.GetComponent<RectTransform>().position = point;
        contextMenu.Target = obj;
        foreach(var action in obj.GetComponent<Interactable>().AvailableActions){
            contextMenu.AddAction(player, action);
        }
    }

    public void CloseContextMenu(){
        if(contextMenuPanel.activeSelf == true){
            contextMenuPanel.SetActive(false);
        }
    }

    #endregion

    #region Examine Text

    public void ShowExamineText(Vector3 point, string text){
        examineTextPanel.SetActive(true);
        examineText.ClearText();
        examineText.SetText(text);
        examineTextPanel.GetComponent<RectTransform>().position = point;
    }

    public void CloseExamineText(){
        if(examineTextPanel.activeSelf == true){
            examineTextPanel.SetActive(false);
        }
    }

    #endregion
}
