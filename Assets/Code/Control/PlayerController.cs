using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

        #region Component Reference Properties

        [SerializeField] Camera currentcam;

        [SerializeField] GameObject GUIContainer;

        [SerializeField] State debugState;

        PlayerHandler handler;

        Inventory inventory;

        #endregion

        #region Properties

        [SerializeField] public State CurrentState {get => handler.currentState; set => handler.SetState(value);}

        // This is the cursor's current position on the screen, will mostly move in k+m only!
        [SerializeField] public Vector2 CursorPosition;

        // Be pretty nice if this was a component eventually wunnit?
        [SerializeField] public Vector3 SelectorPosition {get => handler.selectorPosition; set => handler.selectorPosition = value;}

        [SerializeField] public GameObject HoveredObject;

        public Interaction CurrentInteraction {get => handler.currentInteraction; set => handler.currentInteraction = value;}

        #endregion

        #region Unity Built-ins

        private void Awake() {
                handler = new PlayerHandler(this.gameObject, currentcam, GUIContainer);

                CurrentState = State.DEFAULT;
                inventory = GetComponent<Inventory>(); 
                CurrentInteraction = null;
        }

        private void Update() {
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
                Debug.Log("Button clicked");

                // State Machine
                handler.HandleState();
                debugState = CurrentState;
        }

        #endregion

        #region Input Handlers

        public void HandleSelect(){
                if(EventSystem.current.IsPointerOverGameObject())
                        return;

                // If a gui option isn't being selected, close the manager window
                var guiman = GUIContainer.GetComponent<GUIManager>();
                guiman.CloseMenus();

                // If an interactable is at the cursor, do its default interaction
                if(HoveredObject.GetComponent<Interactable>() != null){
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
                        Debug.Log(HoveredObject.name);
                        guiman.OpenContextMenu(CursorPosition, HoveredObject);
                }
        }

        public void Sheathe(){
                Debug.Log("Sheathing weapons");
                inventory.Swap();
                handler.UpdateInventoryPositions();
        }

        public void Drop(){
                if(inventory.HasItemInHand()){
                        var handobj = inventory.Hand1;
                        inventory.Clear(handobj);
                        handobj.transform.position = this.transform.position;
                        handobj.transform.parent = null;
                        handobj.layer = 6;
                }
        }

        #endregion
}