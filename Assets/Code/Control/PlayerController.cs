using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Holds the logic for passing information into the Actor Controller for a player
public class PlayerController : MonoBehaviour
{
        #region Field Declarations

        #region Input fields

        public GameObject HoveredObject;

        Vector3 SelectorPosition;

        public Vector2 CursorPosition;

        public InputType CurrentInput;

        #endregion

        #region Camera fields

        // *** Move to CameraControl ***
        GameObject cameraTarget;

        [SerializeField] float CamRotateVelocity;
        [SerializeField] float CamZoomVelocity;

        float CurrentCamRotate = 0f;
        float CurrentCamZoom = 0f;
        [SerializeField] GameObject LookAtTarget;
        // ***
        [SerializeField] Camera mainCam;

        CameraControl cameraControl;

        public Vector3 CameraInput;

        #endregion

        #region UI fields

        [SerializeField] GameObject guiControlObject;

        GUIManager guiController;
        
        #endregion

        #region Player fields

        public ActionType MaskedAction;

        static ActionType defaultAction = ActionType.Default;

        ActorController controller;

        #endregion
        
        #endregion

        #region Unity Built-ins

        private void Awake() {
                cameraControl = mainCam.GetComponent<CameraControl>();
        }

        private void Update() {
                // Send updates to the camera
                // *** Needs update - most of it could be moved to CameraControl
                cameraTarget.transform.position = transform.position;
                cameraControl.OffsetRotation(CurrentCamRotate);
                cameraControl.OffsetZoom(CurrentCamZoom);
                // ***

                // Get the raycast for selection purposes
                var ray = mainCam.ScreenPointToRay(CursorPosition);
                RaycastHit hit;
                if(!EventSystem.current.IsPointerOverGameObject()){
                        if(Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 6 | 1 << 9))){
                                // Probably want to put something about layer masks in here
                                if(hit.collider == null){
                                }
                                else {
                                        SelectorPosition = hit.point;
                                        HoveredObject = hit.collider.gameObject;
                                }
                        }

                        // Set the target position for movement related events
                        controller.TargetPos = hit.point;

                        // Set LookAt Target for Proc Animations
                        controller.PALookAt = hit.point + new Vector3(0f, .5f, 0f);
                }

                // Update the selected action
                var selectorAction = GetSelectorAction();

                // Process Input
                switch(CurrentInput){
                        case InputType.Select:
                                if(EventSystem.current.IsPointerOverGameObject()) break;

                                // If not clicking a button, close the GUI Manager window

                                // Add the currently selected action
                                if(HoveredObject == null) {controller.AddAction(ActionType.Walk); break;}
                                controller.AddInteraction(new Interaction(selectorAction, HoveredObject), true);

                                guiController.CloseMenus();

                        break;

                        case InputType.OpenContextMenu:
                                guiController.CloseMenus();
                                guiController.OpenContextMenu(this.gameObject, CursorPosition, HoveredObject);
                        break;

                        case InputType.Drop:
                                controller.DropItem();
                        break;

                        case InputType.Exit:
                                // Needs to be handled in the client manager most likely
                        break;

                        case InputType.Sheathe:
                                controller.SheatheWeapons();
                        break;

                        case InputType.ResetCamera:
                                cameraControl.ResetCamera();
                        break;
                }

                guiController.SetCursorSprite(selectorAction);
        }

        private void LateUpdate() {
                // Update GUI
                if(controller.CurrentState == State.INTERACTING){
                        var interaction = controller.CurrentInteraction;
                        switch(interaction.Type){
                                case ActionType.Store:
                                case ActionType.Grab:
                                        guiController.ContainerRefresh();
                                break;

                                case ActionType.Open:
                                        guiController.ShowContainerInventory(interaction.Target);
                                        guiController.ContainerRefresh();
                                break;

                                case ActionType.Examine:
                                        guiController.ShowExamineText(interaction.Target.transform.position, interaction.Target.GetComponent<Interactable>().ExamineText);
                                break;
                        }
                }
                guiController.ClearPlayerInventory();
                guiController.SetSlotIcon()

        }

        #endregion

        #region Camera functions



        #endregion

        #region UI functions

        ActionType GetSelectorAction(){
                if(MaskedAction != defaultAction) return MaskedAction;
                if(HoveredObject.GetComponent<Interactable>() == null) return ActionType.Walk;
                return HoveredObject.GetComponent<Interactable>().AvailableActions[0];
        }

        #endregion

        #region Input Functions

        #endregion
}