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

    [SerializeField] GameObject inventoryPanel;


    [SerializeField] GameObject IconTemplate;

    List<GameObject> inventoryIcons;

    GameObject MenuTarget;

    [SerializeField] GameObject Player;

    #endregion

    #region Unity Functions

    private void Awake() {
        contextMenu = GetComponent<ContextMenu>();
        examineText = GetComponent<ExamineText>();
        CloseMenus();

        inventoryIcons = new List<GameObject>();
    }

    private void Update() {
    }

    #endregion

    #region Generic Functions

    public void CloseMenus()
    {
        CloseContextMenu();
        CloseExamineText();
        CloseContainerInventory();
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
            contextMenu.AddAction(Player, action);
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

    #region Container Inventory

    public void ShowContainerInventory(GameObject target){
        inventoryPanel.SetActive(true);
        MenuTarget = target;
        ContainerRefresh();
    }   

    public void CloseContainerInventory(){
        inventoryPanel.SetActive(false);
    }

    public void ContainerRefresh(){
        ClearInventoryIcons();

        foreach(var item in MenuTarget.GetComponent<Container>().StoredItems){
            AddInventoryIcon(item, item.GetComponent<Interactable>().Image);
        }
    }

    void AddInventoryIcon(GameObject item, Sprite image){
        var icon = Instantiate(IconTemplate);
        icon.transform.SetParent(inventoryPanel.transform);
        icon.GetComponent<InventoryIcon>().SetupButton(Player, image, item);
        inventoryIcons.Add(icon);
    }

    void ClearInventoryIcons(){
        foreach(var item in inventoryIcons){
            Debug.Log("Destroying" + item);
            Destroy(item);
        } 
        inventoryIcons.Clear();
    }

    public void RemoveItem(GameObject item){
        MenuTarget.GetComponent<Container>().StoredItems.Remove(item);
        ContainerRefresh();
    }

    #endregion
}
