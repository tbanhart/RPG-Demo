using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Container for the state machine logic of a generic actor
public abstract class ConsumableHandler : IStateHandler
{
    #region Properties

    // Relevant Components
    public ActorAnimator animator { get; set; }

    public Movement movement { get; set; }

    public Inventory inventory { get; set; }

    public Combat combat { get; set; }

    public Interaction currentInteraction { 
        get {
            if (_currentInteraction == null) return null;
            else return _currentInteraction;
        } 
        set => _currentInteraction = value; 
    }

    Interaction _currentInteraction = null;

    // Used properties
    public GameObject Owner { get; set; }

    public Vector3 selectorPosition { get; set; }

    public State currentState { get; set; }

    public GameObject handPosition { get => inventory.HandLocation; }

    public GameObject sheathePosition { get => inventory.EquipLocation; }

    public Camera maincam { get; set; }

    public GameObject guiContainer { get; set; }

    #endregion

    public ConsumableHandler(GameObject owner, Camera cam, GameObject gui){


        // Set entered objects from owner
        Owner = owner;
        maincam = cam;
        guiContainer = gui;

        // Set components from owner
        animator = owner.GetComponent<ActorAnimator>();
        movement = owner.GetComponent<Movement>();
        inventory = owner.GetComponent<Inventory>();
        combat = owner.GetComponent<Combat>();
    
        // Set properties to default values
        currentInteraction = null;
        currentState = State.IDLE;
        selectorPosition = Vector3.zero;
    }

    #region Inherited standard state handlers

    public void HandleState(){
        switch(currentState){
            case State.IDLE:
                HandleIdle(); break;
            
            case State.MOVING:
                HandleMove(); break;
            
            case State.TRAVELLING:
                HandleTravelling(); break;

            case State.ATTACKING:
                HandleTravelling(); break;

            case State.INTERACTING:
                HandleInteracting(); break;
        }
    }

    public void HandleIdle(){
        if (currentInteraction != null){
            SetState(State.TRAVELLING);
        }   
    }

    public void HandleMove(){
        if(movement.IsMoving == false) {SetState(State.IDLE); return;}

        animator.SetMovement(movement.GetVelocity());
    }

    public void HandleAttack(){
        var stage = combat.attackStage;

        // Stage: Starting attack
        if(stage == AttackStage.START){
            // Check attack range
            if(movement.GetDistance(currentInteraction.Target.transform.position) > combat.Range){
                movement.SetDestination(currentInteraction.Target.transform.position);
                SetState(State.TRAVELLING);
                movement.SetStoppingDistance(combat.Range);
                animator.SetAttacking(false);
                return;
            }

            // Set animator
            animator.SetAttacking(true);
        }
        #region Unused stages
        // Stage: Doing the windup
        else if(stage == AttackStage.WINDUP){
        }
        // Stage: Dealing damage
        else if(stage == AttackStage.DAMAGE){
            // Create damage collision object
        }
        // Stage: Cooling down
        else if(stage == AttackStage.COOLDOWN){
        }
        #endregion
        // Stage: End
        else if(stage == AttackStage.END){
            animator.SetAttacking(false);
            SetState(State.IDLE);
            return;
        }
        
        // Adding this as a replacement for doattack since most of that is handled here
        combat.ProgressAttack();
    }

    public void HandleTravelling(){
        // If the move is complete or canceled, do the interaction
        if (movement.IsMoving == false)
        {
            SetState(currentInteraction.Type);
            animator.SetMovement(Vector3.zero);
        }
        else
        {
            animator.SetMovement(movement.GetVelocity());
        }
    }

    public void HandleInteracting(){
        DoInteraction();
    }

    public void HandleInventory(){
        
    }

    #endregion

    #region State Setters

    public void SetState(State state){
        currentState = state;

        switch(currentState){
            case State.IDLE:
            Debug.Log("Idle");
                animator.SetMovement(Vector3.zero);
                break;

            case State.MOVING:
            case State.TRAVELLING:
                movement.SetDestination(selectorPosition);
                break;

            case State.ATTACKING:
                animator.SetMovement(Vector3.zero);
                combat.StartAttack();
                break;

            case State.INVENTORY:
                animator.SetMovement(Vector3.zero);
                break;
        }
    }

    public void SetState(ActionType action)
    {
        switch (action)
        {
            case ActionType.Attack:
                SetState(State.ATTACKING);
                break;
            case ActionType.Examine:
            case ActionType.Grab:
            case ActionType.Open:
            case ActionType.Store:
                SetState(State.INTERACTING);
                break;
            default:
                Debug.Log("Not sure how to handle the actiontype: " + action);
                break;
        }
    }

    #endregion

    #region Accessor functions - no need to override

    public void UpdateSelector(Vector3 position){
        selectorPosition = position;
    }

    #endregion

    public void DoInteraction()
    {
        switch (currentInteraction.Type)
        {
            case ActionType.Grab:
                var didpickup = inventory.Pickup(currentInteraction.Target);
                if (didpickup)
                {
                    //CurrentInteraction.Target.GetComponent<Interactable>().Grabbable = false;
                    //Debug.Log("Picking up " + CurrentInteraction.Target.name);
                    currentInteraction.Target.layer = 2;
                    UpdateInventoryPositions();
                }
                CompleteInteraction();
                break;

            case ActionType.Examine:
            // ***Later interactions will take a duration to complete, for now it just handles them like they're all instantaneous ***
                var text = currentInteraction.Target.GetComponent<Interactable>().ExamineText;
                if (text != string.Empty)
                {
                    var point = currentInteraction.Target.transform.position;
                    guiContainer.GetComponent<GUIManager>().ShowExamineText(maincam.WorldToScreenPoint(point), text);
                }
                CompleteInteraction();
                break;

            case ActionType.Store:
                if(!inventory.HasItemInHand()) break;
                var item = inventory.Hand1;
                bool itemstored;
                itemstored = 
                    currentInteraction.Target.GetComponent<Container>().
                        StoreItem(item);
                if(itemstored == true){
                    item.transform.SetParent(currentInteraction.Target.transform);
                    item.layer = 2;
                    item.GetComponent<MeshRenderer>().enabled = false;
                    inventory.Clear(item);
                    UpdateInventoryPositions();
                }
                CompleteInteraction();
                break;

            case ActionType.Open:
                guiContainer.GetComponent<GUIManager>().ShowContainerInventory(currentInteraction.Target);
                SetState(State.INVENTORY);
                CompleteInteraction();
                break;

            default:
                Debug.Log("Unhandled interaction actiontype " + currentInteraction.Type);
                break;
        }
    }

    // *** This needs to be moved to the inventory component ***
    public void UpdateInventoryPositions()
    {
        if (inventory.HasItemInHand() == true)
        {
            var invhand = inventory.Hand1;
            var invinter = invhand.GetComponent<Interactable>();
            var rot = invinter.HandOffsetRot;
            invhand.transform.parent = handPosition.transform;
            invhand.transform.localPosition = invinter.HandOffsetPos;
            invhand.transform.localRotation = new Quaternion();
            invhand.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        if (inventory.HasItemEquipped() == true)
        {
            var invequip = inventory.Equip1;
            var invinter = invequip.GetComponent<Interactable>();
            var rot = invinter.EquipOffsetRot;
            invequip.transform.parent = sheathePosition.transform;
            invequip.transform.localPosition = invinter.EquipOffsetPos;
            invequip.transform.localRotation = new Quaternion();
            invequip.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        if (inventory.HasEquipment() == true){
            var invequip = inventory.Equip2;
            var invinter = invequip.GetComponent<Interactable>();
            var rot = invinter.EquipOffsetRot;
            invequip.transform.parent = sheathePosition.transform;
            invequip.transform.localPosition = invinter.EquipOffsetPos;
            invequip.transform.localRotation = new Quaternion();
            invequip.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    public void CompleteInteraction(){
        currentInteraction = null;
        SetState(State.IDLE);
    }
}
