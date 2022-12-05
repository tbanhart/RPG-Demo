using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    #region Component Reference Properties

    [SerializeField] Camera currentcam;

    [SerializeField] GameObject GUIContainer;

    GUIManager guiManager;

    CameraControl cameraControl;

    ConsumableHandler handler;

    Inventory inventory;

    #endregion

    #region Properties

    [SerializeField] public State CurrentState {get => handler.currentState; set => handler.SetState(value);}

    // This is the cursor's current position on the screen, will mostly move in k+m only!
    [SerializeField] public Vector2 CursorPosition;

    // Be pretty nice if this was a component eventually wunnit?
    [SerializeField] public Vector3 SelectorPosition {get => handler.selectorPosition; set => handler.selectorPosition = value;}

    public GameObject HoveredObject;

    public Interaction CurrentInteraction {get => handler.currentInteraction; set => handler.currentInteraction = value;}

    [SerializeField] float CamRotateVelocity;

    [SerializeField] float CamZoomVelocity;

    float CurrentCamRotate = 0f;

    float CurrentCamZoom = 0f;

    GameObject CameraTarget;

    [SerializeField] GameObject LookAtTarget;

    ActionType maskedAction;

    Encumberance encumberance {get => handler.encumberance; set => handler.encumberance = value;}

    CarryState carryState {get => handler.carryState; set => handler.carryState = value;}

    [SerializeField] float CarryOneHand;

    [SerializeField] float MaxCarryWeight;

    [SerializeField] State debugState;

    [SerializeField] Encumberance debugEncumberance;

    [SerializeField] CarryState debugCarryState;

    #endregion

    #region Unity Built-ins

    private void Awake() {
        handler = new PlayerHandler(this.gameObject, currentcam, GUIContainer);

        CurrentState = State.IDLE;
        inventory = GetComponent<Inventory>(); 
        cameraControl = GetComponent<CameraControl>();
        CurrentInteraction = null;
        CameraTarget = new GameObject();
        currentcam.transform.SetParent(CameraTarget.transform);
        cameraControl.CameraTarget = CameraTarget;
        cameraControl.ResetCamera();
        guiManager = GUIContainer.GetComponent<GUIManager>();
        handler.CarryOneHand = CarryOneHand;
        handler.MaxCarryWeight = MaxCarryWeight;
        ClearActionMask();   
        UpdateCarryWeights();             
    }

    private void Update() {
        // Send updates to the camera
        CameraTarget.transform.position = transform.position;
        cameraControl.OffsetRotation(CurrentCamRotate);
        cameraControl.OffsetZoom(CurrentCamZoom);

        // Get the raycast for selection purposes
        var ray = currentcam.ScreenPointToRay(CursorPosition);
        RaycastHit hit;
        if(!EventSystem.current.IsPointerOverGameObject())
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 6 | 1 << 9))){
            // Probably want to put something about layer masks in here
            if(hit.collider == null){
            }
            else {
                    SelectorPosition = hit.point;
                    HoveredObject = hit.collider.gameObject;
            }
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 5)))
                Debug.Log("Button Clicked");

        // State Machine
        handler.HandleState();

        // Set LookAt Target for Proc Animations
        if(CurrentInteraction == null) SetLookAt(new Vector3(SelectorPosition.x, 0f, SelectorPosition.z));
        else SetLookAt(CurrentInteraction.Target);

        UpdateCursorSprite();


        debugState = CurrentState;
        debugEncumberance = encumberance;
        debugCarryState = carryState;
    }

    #endregion

    #region Input Handlers

    public void HandleSelect(){
        // Make the UI mask input
        if(EventSystem.current.IsPointerOverGameObject()) return;
                
        // If a gui option isn't being selected, close the manager window
        var guiman = GUIContainer.GetComponent<GUIManager>();
        guiman.CloseMenus();

        // If an interactable is at the cursor, do its default interaction
        if (maskedAction != ActionType.Walk &&
        HoveredObject != null && HoveredObject.GetComponent<Interactable>() != null)
        {
            ActionType action;
            if (maskedAction != ActionType.Default)
            {
                Debug.Log(maskedAction);
                action = maskedAction;
            }
            else
            {
                action = HoveredObject.GetComponent<Interactable>().AvailableActions[0];
            }

            switch (action)
            {
                case ActionType.Attack:
                    float damage;
                    if (inventory.Hand1 == null) damage = 1f;
                    else damage = inventory.Hand1.GetComponent<Interactable>().AttackDamage;
                        CurrentInteraction = new Interaction(action, HoveredObject, 3f, damage, 3f);
                    break;
                    
                default:
                    CurrentInteraction = new Interaction(action, HoveredObject, 3f);
                    break;
            }
        }
        // Other wise move to the selected point
        else
        {
            // Start moving to the selected point
            handler.SetState(State.MOVING);
            CurrentInteraction = null;
        }

        ClearActionMask();
    }

    public void HandleOpenContext(){
        if (EventSystem.current.IsPointerOverGameObject())
                return;

        // If a gui option isn't being selected, close the manager window
        var guiman = GUIContainer.GetComponent<GUIManager>();
        guiman.CloseMenus();

        // If an interactable is being selected, open the context menu
        if (HoveredObject.GetComponent<Interactable>() != null)
        {
             guiman.OpenContextMenu(this.gameObject, CursorPosition, HoveredObject);
        }
    }

    public void Sheathe(){
            Debug.Log("Sheathing weapons");
            inventory.Swap();
            handler.UpdateInventoryPositions();
    }

    public void Drop(){
        // *** These could be consolidated into one execution ***
        // *** These should also be moved to inventory maybe? ***
        if(inventory.HasItemInHand()){
            var handobj = inventory.Hand1;
            var startingrot = handobj.GetComponent<Interactable>().DefaultRotation;
            inventory.Clear(handobj);
            handobj.transform.position = this.transform.position;
            handobj.transform.parent = null;
            handobj.transform.rotation = new Quaternion();
            handobj.transform.localRotation = Quaternion.Euler(startingrot.x, startingrot.y, startingrot.z);
            handobj.layer = 6;
        } else if(inventory.HasItemEquipped() || inventory.HasEquipment()){
            var obj = inventory.Equip2;
            var startingrot = obj.GetComponent<Interactable>().DefaultRotation;
            inventory.Clear(obj);
            obj.transform.rotation = new Quaternion();
            obj.transform.localRotation = Quaternion.Euler(startingrot.x, startingrot.y, startingrot.z);
            obj.transform.position = this.transform.position;
            obj.transform.parent = null;
            obj.layer = 6;
        }
                
        handler.UpdateInventoryPositions();
    }

    public void RotateCamera(float offset){
            CurrentCamRotate = offset * CamRotateVelocity;
    }

    public void ZoomCamera(float offset){
            CurrentCamZoom = offset * CamZoomVelocity;
    }

    public void ResetCamera(){
            cameraControl.ResetCamera(this.gameObject.transform);
    }

    public void Exit(){
            Application.Quit();
    }

    public void SetActionMask(ActionType actionType){
            maskedAction = actionType;
    }
        
    #endregion

    public void AddInteraction(ActionType action, GameObject target){
            GUIContainer.GetComponent<GUIManager>().CloseContextMenu();
            CurrentInteraction = new Interaction(action, target);
            handler.SetState(action);
    }

    public void SetLookAt(GameObject obj){
            LookAtTarget.transform.position = obj.transform.position;
    }

    public void SetLookAt(Vector3 target){
            LookAtTarget.transform.position = target;
    }

    public bool TryGrabItem(GameObject item){
            if(inventory.HasItemInHand()){ Debug.Log(inventory.Hand1); return false;}
            Debug.Log("Grabbing item");
            item.GetComponent<MeshRenderer>().enabled = true;
            handler.SetState(ActionType.Grab);
            handler.currentInteraction = new Interaction(
                    ActionType.Grab,
                    item
            );
            guiManager.RemoveItem(item);
            return true;
    }

    void ClearActionMask(){
            maskedAction = ActionType.Default;
    }

    void UpdateCursorSprite(){
            ActionType action;
            if(maskedAction != ActionType.Default) action = maskedAction;
            else{
                    if(HoveredObject.GetComponent<Interactable>() == null) action = ActionType.Walk;
                    else {
                            action = HoveredObject.GetComponent<Interactable>().AvailableActions[0];
                    }
            }
            guiManager.SetCursorSprite(action);
    }

    void UpdateCarryWeights(){
            var weight = inventory.CurrentWeight;
            var inhand = inventory.CarriedWeight;
                
            // *** A Loop here would be really clever ***
            if(inhand <= CarryOneHand) carryState = CarryState.OneHand;
            else if (inhand <= CarryOneHand * 2) carryState = CarryState.TwoHand;
            else carryState = CarryState.Drag;
        
            if(weight <= MaxCarryWeight/2) encumberance = Encumberance.Light;
            else if(weight <= MaxCarryWeight) encumberance = Encumberance.Medium;
            else if (weight <= MaxCarryWeight * 1.5) encumberance = Encumberance.Heavy;
            else encumberance = Encumberance.OverEncumber;
    }

    public void UpdateCursorPosition(Vector2 position){
            CursorPosition = position;
    }
}