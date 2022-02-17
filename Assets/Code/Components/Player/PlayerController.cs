using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

        #region Component Reference Properties

        [SerializeField] Camera currentcam;

        [SerializeField] GameObject GUIContainer;

        [SerializeField] GameObject _rhand;

        [SerializeField] GameObject _equipslot;

        Movement movement;

        PlayerAnimator animator;

        Combat combat;

        Inventory inventory;

        #endregion

        #region Properties

        [SerializeField] public State CurrentState;

        // This is the cursor's current position on the screen, will mostly move in k+m only!
        [SerializeField] public Vector2 CursorPosition;

        // Be pretty nice if this was a component eventually wunnit?
        [SerializeField] public Vector3 SelectorPosition;

        [SerializeField] public GameObject HoveredObject;

        public Interaction CurrentInteraction;

    #endregion

    #region Unity Built-ins

        private void Awake() {
                CurrentState = State.DEFAULT;
                movement = GetComponent<Movement>();
                animator = GetComponent<PlayerAnimator>();
                combat = GetComponent<Combat>();               
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
                switch(CurrentState){

                        #region IDLE: When in between interactions
                        // Check for assigned interaction
                        case State.IDLE:
                        if(CurrentInteraction != null){
                                Debug.Log(CurrentInteraction);
                                // Create a movement path
                                movement.SetDestination(CurrentInteraction.Target.transform.position);
                                movement.SetStoppingDistance(CurrentInteraction.Distance);

                                // If player is there already, run the interaction code
                                if(movement.IsMoving == false){
                                        SetState(CurrentInteraction.Type);
                                } 
                                // Otherwise start moving to the destination
                                else SetState(State.TRAVELLING);
                        }
                        // Fallthrough statement for if the player is stuck here but should be in move state
                        else if(movement.IsMoving == true){
                                SetState(State.MOVING);
                        }
                        break;
                        #endregion
                        
                        #region MOVING: For if movement was the last thing designated
                        // *** This can be combined into one state using move to as an interaction ***
                        case State.MOVING:
                                // If the move is complete or canceled, back we go to idle
                                if(movement.IsMoving == false){
                                        SetState(State.IDLE);
                                        animator.SetMovement(Vector3.zero);
                                } else{
                                        animator.SetMovement(movement.GetVelocity());
                                }
                                break;
                        #endregion

                        #region TRAVELLING: For in transit to interaction destination
                        case State.TRAVELLING:
                                // If the move is complete or canceled, do the interaction
                                if(movement.IsMoving == false){
                                        SetState(CurrentInteraction.Type);
                                        animator.SetMovement(Vector3.zero);
                                } else{
                                        animator.SetMovement(movement.GetVelocity());
                                }
                                break;
                        #endregion

                        #region ATTACKING: In range and executing an attack interaction
                        case State.ATTACKING:
                                // Check if attack range got messed up
                                if(movement.GetDistance(CurrentInteraction.Target.transform.position) > combat.Range){
                                        CurrentInteraction.Distance = combat.Range;
                                        SetState(State.TRAVELLING);
                                } else{
                                        // Deal damage and return if the target is dead
                                        var isDead = combat.DoAttack(CurrentInteraction.Target.GetComponent<Combat>());
                                        animator.SetAttacking(true);
                                        if(isDead == true){
                                                CurrentInteraction.Target.GetComponent<Combat>().Kill(this.name);
                                                CurrentInteraction = null;
                                                SetState(State.IDLE);
                                                animator.SetAttacking(false);
                                        }
                                }
                                break;
                        #endregion

                        #region INTERACTING: Doing an interaction on a target within range
                        case State.INTERACTING:
                                // ***Proc animation would go here for reaching out for the object ***
                                /*
                                // Go back to traveling if target moved out of range
                                if(movement.GetDistance(CurrentInteraction.Target) > CurrentInteraction.Distance){
                                        movement.SetStoppingDistance(CurrentInteraction.Distance);
                                        movement.SetDestination(CurrentInteraction.Target);
                                        SetState(State.TRAVELLING);
                                }
                                */
                                // Progress interaction
                                //else{
                                        //Debug.Log("Progressing interaction, progress: " + CurrentInteraction.Progress);
                                        //if(CurrentInteraction.Progress <= 0f){
                                                CurrentState = State.IDLE;
                                                //SetState(State.IDLE);
                                                DoInteraction();
                                                Debug.Log("Grabbed");
                                                CurrentInteraction = null;
                                        //} else CurrentInteraction.Progress -= 1f;
                                //}
                                break;
                        #endregion
                }
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
                        SetState(State.MOVING);
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
                UpdateInventoryPositions();
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

        #region State Handlers?

        void SetState(State state){
                CurrentState = state;

                // Optional additional steps
                switch(state){
                        case State.MOVING:
                        case State.TRAVELLING:
                                movement.SetDestination(SelectorPosition);
                                break;
                }
        }

        void SetState(ActionType action){
                switch(action){
                        case ActionType.Attack:
                                SetState(State.ATTACKING);
                                break;

                        case ActionType.Examine:
                        case ActionType.Grab:
                                SetState(State.INTERACTING);
                                break;

                        default:
                                Debug.Log("Not sure how to handle the actiontype: " + action);
                                break;
                }
        }

        #endregion

        #region Misc functions

        public void DoInteraction(){
                switch(CurrentInteraction.Type){
                        case ActionType.Grab:
                                var didpickup = inventory.Pickup(CurrentInteraction.Target);
                                if(didpickup){
                                        //CurrentInteraction.Target.GetComponent<Interactable>().Grabbable = false;
                                        //Debug.Log("Picking up " + CurrentInteraction.Target.name);
                                        CurrentInteraction.Target.layer = 2;
                                        UpdateInventoryPositions();
                                }
                                break;  

                        case ActionType.Examine:
                                var text = CurrentInteraction.Target.GetComponent<Interactable>().ExamineText;
                                if(text != string.Empty){
                                        var point = CurrentInteraction.Target.transform.position;
                                        GUIContainer.GetComponent<GUIManager>().ShowExamineText(currentcam.WorldToScreenPoint(point), text);
                                }
                                break;

                        default:
                                Debug.Log("Unhandled interaction actiontype " + CurrentInteraction.Type);
                                break;
                }
        }

        public void UpdateInventoryPositions(){
            if (inventory.HasItemInHand() == true)
            {
                Debug.Log("Moving hand inv");
                var invhand = inventory.Hand1;
                invhand.transform.parent = _rhand.transform;
                invhand.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            if (inventory.HasItemEquipped() == true)
            {
                Debug.Log("Moving equipped inv");
                var invequip = inventory.Equip1;
                invequip.transform.parent = _equipslot.transform;
                invequip.transform.localPosition = new Vector3(0f, -.4f, 0f);
            }
        }

        #endregion
}