using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

        #region Component Reference Properties

        [SerializeField] Camera currentcam;

        [SerializeField] GameObject GUIContainer;

        [SerializeField] State debugState;

        CameraControl cameraControl;

        PlayerHandler handler;

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
                debugState = CurrentState;
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
                if(HoveredObject != null && HoveredObject.GetComponent<Interactable>() != null){
                        var action = HoveredObject.GetComponent<Interactable>().AvailableActions[0];
                        CurrentInteraction = new Interaction(
                                action, 
                                HoveredObject,
                                3f);
                }
                // Other wise move to the selected point
                else{
                        // Start moving to the selected point
                        handler.SetState(State.MOVING);
                        CurrentInteraction = null;
                }
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
                // *** These could be convinced into one execution ***
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
                } else if(inventory.HasItemEquipped()){
                        var obj = inventory.Equip2;
                        var startingrot = obj.GetComponent<Interactable>().DefaultRotation;
                        inventory.Clear(obj);
                        obj.transform.rotation = new Quaternion();
                        obj.transform.localRotation = Quaternion.Euler(startingrot.x, startingrot.y, startingrot.z);
                        obj.transform.position = this.transform.position;
                        obj.transform.parent = null;
                        obj.layer = 6;
                }
        }

        public void RotateCamera(float offset){
                CurrentCamRotate = offset * CamRotateVelocity;
        }

        public void ZoomCamera(float offset){
                CurrentCamZoom = offset * CamZoomVelocity;
        }

        public void AddInteraction(ActionType action, GameObject target){
                GUIContainer.GetComponent<GUIManager>().CloseContextMenu();
                CurrentInteraction = new Interaction(action, target);
                handler.SetState(action);
        }

        #endregion
}