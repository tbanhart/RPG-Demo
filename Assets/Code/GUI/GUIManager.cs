using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    #region Reference Objects

    #region Context Menu

    [SerializeField] GameObject contextMenuPanel;

    ContextMenu contextMenu;

    #endregion

    #region Examine Text

    [SerializeField] GameObject examineTextPanel;

    ExamineText examineText;

    #endregion

    #region Container Inventory

    [SerializeField] GameObject containerPanel;


    [SerializeField] GameObject IconTemplate;

    List<GameObject> containerInventory;

    #endregion

    #region Player Inventory

    [SerializeField] GameObject InventoryPanel;

    [SerializeField] GameObject HandsIcon;

    [SerializeField] GameObject SheatheIcon;

    [SerializeField] GameObject EquipmentIcon;

    [SerializeField] Sprite DefaultIcon;

    #endregion

    #region

    [SerializeField] StatBar _progressBar;

    #endregion

    #region Window Agnostic

    GameObject MenuTarget;

    [SerializeField] GameObject Player;

    [SerializeField] Texture2D IconExamine;
    [SerializeField] Texture2D IconAttack;
    [SerializeField] Texture2D IconMove;
    [SerializeField] Texture2D IconGrab;
    [SerializeField] Texture2D IconStore;
    [SerializeField] Texture2D IconOpen;

    List<Texture2D> ActionIcons;

    int CursorSprite;

    #endregion

    #endregion

    #region Unity Functions

    private void Awake() {
        contextMenu = GetComponent<ContextMenu>();
        examineText = GetComponent<ExamineText>();
        CloseMenus();

        containerInventory = new List<GameObject>();
        ClearPlayerInventory();


        ActionIcons = new List<Texture2D>();
        ActionIcons.Insert((int)ActionType.Examine, IconExamine);
        ActionIcons.Insert((int)ActionType.Attack, IconAttack);
        ActionIcons.Insert((int)ActionType.Walk, IconMove);
        ActionIcons.Insert((int)ActionType.Grab, IconGrab);
        ActionIcons.Insert((int)ActionType.Store, IconStore);
        ActionIcons.Insert((int)ActionType.Open, IconOpen);
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
        CloseProgressBar();
    }

    public void SetCursorSprite(ActionType action){
        if(action == ActionType.Default) return;
        if(ActionIcons[(int)action] == null)
            Cursor.SetCursor(null, new Vector2(0f,0f), CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(ActionIcons[(int)action], new Vector2(ActionIcons[(int)action].width/2, ActionIcons[(int)action].height/2), CursorMode.Auto);
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

    #region Progress Bar

    public void EnableProgressBar()
    {
        _progressBar.enabled = true;
        _progressBar.SliderValue = 0f;
    }

    public void CloseProgressBar()
    {
        _progressBar.SliderValue = 0f;
        _progressBar.enabled = false;
    }

    public void UpdateProgressBar(float value)
    {
        _progressBar.SliderValue = value;
    }

    #endregion

    #region Container Inventory

    public void ShowContainerInventory(GameObject target){
        containerPanel.SetActive(true);
        MenuTarget = target;
        ContainerRefresh();
    }   

    public void CloseContainerInventory(){
        containerPanel.SetActive(false);
    }

    public void ContainerRefresh(){
        ClearInventoryIcons();

        foreach(var item in MenuTarget.GetComponent<Container>().StoredItems){
            AddInventoryIcon(item, item.GetComponent<Interactable>().Image);
        }
    }

    void AddInventoryIcon(GameObject item, Sprite image){
        var icon = Instantiate(IconTemplate);
        icon.transform.SetParent(containerPanel.transform);
        icon.GetComponent<InventoryIcon>().SetupButton(Player, image, item);
        containerInventory.Add(icon);
    }

    void ClearInventoryIcons(){
        foreach(var item in containerInventory){
            Debug.Log("Destroying" + item);
            Destroy(item);
        } 
        containerInventory.Clear();
    }

    public void RemoveItem(GameObject item){
        MenuTarget.GetComponent<Container>().StoredItems.Remove(item);
        ContainerRefresh();
    }

    #endregion

    #region Slot Icon

    public void SetSlotIcon(InventorySlot slot, Sprite icon){
        GameObject invicon;
        
        // *** Maybe a property for the sprites of the slot objects? ***
        switch(slot){
            case InventorySlot.HAND1:
                HandsIcon.GetComponent<Image>().sprite = icon;
                invicon = HandsIcon;
                break;

            case InventorySlot.EQUIP1:
                invicon = SheatheIcon;
                break;

            case InventorySlot.EQUIP2:
                invicon = EquipmentIcon;
                break;

            default:
                invicon = HandsIcon;
                break;
        }

        invicon.GetComponent<Image>().sprite = icon;
    }

    #endregion

    #region Player Inventory

    public void ClearPlayerInventory(){
        HandsIcon.GetComponent<Image>().sprite = DefaultIcon;
        SheatheIcon.GetComponent<Image>().sprite = DefaultIcon;
        EquipmentIcon.GetComponent<Image>().sprite = DefaultIcon;
    }

    #endregion
}
