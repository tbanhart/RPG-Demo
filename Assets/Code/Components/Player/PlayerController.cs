using UnityEngine;

public class PlayerController : MonoBehaviour {

        #region Component Reference Properties

        [SerializeField] Camera currentcam;

        [SerializeField] GameObject GUIContainer;

        Movement movement;

        PlayerAnimator animator;

        #endregion

        #region Properties

        [SerializeField] public State CurrentState;

        // This is the cursor's current position on the screen, will mostly move in k+m only!
        [SerializeField] public Vector2 CursorPosition;

        // Be pretty nice if this was a component eventually wunnit?
        [SerializeField] public Vector3 SelectorPosition;

        [SerializeField] public GameObject HoveredObject;

        #endregion

        #region Unity Built-ins

        private void Awake() {
                CurrentState = State.DEFAULT;
                movement = GetComponent<Movement>();
                animator = GetComponent<PlayerAnimator>();
        }

        private void Update() {
                // Get the raycast for selection purposes
                var ray = currentcam.ScreenPointToRay(CursorPosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 6 | 1 << 9))){
                        // Probably want to put something about layer masks in here
                        if(hit.collider == null){
                        }
                        else {
                                SelectorPosition = hit.point;
                                HoveredObject = hit.collider.gameObject;
                        }
                }
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

                // State Machine
                switch(CurrentState){
                        
                        // For if movement was the last thing designated
                        case State.MOVING:
                                // If the move is complete or canceled, back we go to idle
                                if(movement.IsMoving == false){
                                        SetState(State.IDLE);
                                        animator.SetMovement(Vector3.zero);
                                } else{
                                        animator.SetMovement(movement.GetVelocity());
                                }
                                
                                break;
                }
        }

        #endregion

        #region Input Handlers

        public void HandleSelect(){
                // If a gui option isn't being selected, close the manager window
                var guiman = GUIContainer.GetComponent<GUIManager>();
                guiman.CloseContextMenu();

                // If an interactable is being selected, open the context menu
                if(HoveredObject.GetComponent<Interactable>() != null){
                        Debug.Log(HoveredObject.name);
                        guiman.OpenContextMenu(CursorPosition, HoveredObject);
                }

                // Start moving to the selected point
                movement.SetDestination(SelectorPosition);      
                SetState(State.MOVING);
        }

        #endregion

        #region State Handlers?

        void SetState(State state){
                CurrentState = state;
        }

        #endregion
}