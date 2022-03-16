using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Process the current state and send instructions to the different actor components
public class ActorController : MonoBehaviour
{
    #region Property declarations

    #region Component references

    Movement movement;
    ActorAnimator animator;
    Inventory inventory;
    Combat combat;

    #endregion

    #region Interaction queueing

    // Gets the interaction and returns null if there isn't one
    public Interaction CurrentInteraction
    {
        get
        {
            if (_currentInteraction == null) return null;
            else return _currentInteraction;
        }
        set => _currentInteraction = value;
    }

    Interaction _currentInteraction;

    public State CurrentState;

    // *** Move to inventory ***
    CarryState carryState;

    Encumberance encumberance;
    // ***
    // *** Move to actor stats ***
    [SerializeField] float CarryOneHand;

    [SerializeField] float MaxCarryWeight;

    public GameObject handPosition { get => inventory.HandLocation; }

    public GameObject sheathePosition { get => inventory.EquipLocation; }

    // ***

    #endregion

    #region GameObject references

    public Vector3 TargetPos;

    #endregion

    #region Procedural animation references

    public Vector3 PALookAt {get; set;}

    [SerializeField] GameObject LookAtObject;

    GameObject LookAtParent {get => LookAtObject.transform.parent.gameObject; set => LookAtObject.transform.SetParent(value.transform);}

    #endregion

    #region Debug fields to display in editor

    [SerializeField] State debugState;

    [SerializeField] Encumberance debugEncumberance;

    [SerializeField] CarryState debugCarryState;

    #endregion

    #endregion

    #region Unity built-ins

    private void Awake() {
        // Set the component references
        movement = GetComponent<Movement>();
        animator = GetComponent<ActorAnimator>();
        inventory = GetComponent<Inventory>();
        combat = GetComponent<Combat>();

        // Initialize fields
        _currentInteraction = null;
        CurrentState = State.IDLE;
    }

    private void Update() {
        switch(CurrentState){
            case State.IDLE:
                if(CurrentInteraction != null){
                    SetState(State.TRAVELLING);
                }
            break;

            case State.MOVING:
                if(movement.IsMoving == false) {SetState(State.IDLE); return;}

                animator.SetMovement(movement.GetVelocity());
            break;

            case State.TRAVELLING:
                if(movement.IsMoving == false){
                    SetState(CurrentInteraction.Type);
                    animator.SetMovement(Vector3.zero);
                }
                else{
                    animator.SetMovement(movement.GetVelocity());
                }
            break;
            
            case State.INTERACTING:
                switch(CurrentInteraction.Phase){
                    case InteractionPhase.Start:
                        CurrentInteraction.Phase = InteractionPhase.Progress;
                    break;

                    case InteractionPhase.Progress:
                        if(ProgressInteraction() == InteractionPhase.Complete) CurrentInteraction.Phase = InteractionPhase.Complete;
                    break;

                    case InteractionPhase.Complete:
                        CompleteInteraction();
                    break;
                }
                CompleteInteraction();
            break;

            case State.ATTACKING:
                Debug.Log("Attacking object, moving on");
                CompleteInteraction();
            break;
        }
    }

    #endregion

    #region State functions

    // Try to set the current state and return if it was successful
    bool SetState(State state){
        var success = false;
        CurrentState = state;

        switch (state)
        {
            case State.IDLE:
                Debug.Log("Idle");
                animator.SetMovement(Vector3.zero);
                break;

            case State.MOVING:
                movement.SetDestination(TargetPos);
                break;

            case State.TRAVELLING:
                movement.SetDestination(CurrentInteraction.Target.transform.position);
                break;

            case State.ATTACKING:
                animator.SetMovement(Vector3.zero);
                combat.StartAttack();
                break;

            case State.INVENTORY:
                animator.SetMovement(Vector3.zero);
                break;
        }

        return success;
    }

    bool SetState(ActionType action){
        var success = false;

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

        return success;
    }

    #endregion

    #region Interaction functions

    // Add interaction to the current queue and return if it is possible
    public bool AddInteraction(Interaction interaction, bool cancelCurrent){
        var success = false;

        if(cancelCurrent == true){
            CurrentInteraction = interaction;
            LookAtParent = interaction.Target;
            success = true;
        }

        else{
            Debug.Log("Interaction queueing not implemented, change interaction in stack trace to cancelCurrent = true");
        }

        return success;
    }

    public bool AddAction(ActionType action){
        var success = false;

        SetState(action);

        return success;
    }

    public InteractionPhase ProgressInteraction(){
        var phase = InteractionPhase.Progress;

        switch(CurrentInteraction.Type){
            case ActionType.Grab:
                var didpickup = inventory.Pickup(CurrentInteraction.Target);
                if (didpickup)
                {
                    CurrentInteraction.Target.layer = 2;
                    UpdateInventoryPositions();
                }
                phase = InteractionPhase.Complete;
            break;

            case ActionType.Store:
                if (!inventory.HasItemInHand()) break;
                var item = inventory.Hand1;
                bool itemstored;
                itemstored =
                    CurrentInteraction.Target.GetComponent<Container>().
                        StoreItem(item);
                if (itemstored == true)
                {
                    item.transform.SetParent(CurrentInteraction.Target.transform);
                    item.layer = 2;
                    item.GetComponent<MeshRenderer>().enabled = false;
                    inventory.Clear(item);
                    UpdateInventoryPositions();
                }
                phase = InteractionPhase.Complete;
            break;
            
            case ActionType.Attack:
                phase = InteractionPhase.Complete;
            break;

            case ActionType.Walk:
                if(movement.IsMoving == false) phase = InteractionPhase.Complete;
            break;
        }

        return phase;
    }

    public void CompleteInteraction(){
        /*
        if(CurrentInteraction != null)
        switch (CurrentInteraction.Type)
        {
            case ActionType.Grab:
                var didpickup = inventory.Pickup(CurrentInteraction.Target);
                if (didpickup)
                {
                    //CurrentInteraction.Target.GetComponent<Interactable>().Grabbable = false;
                    //Debug.Log("Picking up " + CurrentInteraction.Target.name);
                    CurrentInteraction.Target.layer = 2;
                    UpdateInventoryPositions();
                }
                CompleteInteraction();
                break;

            case ActionType.Examine:
                // ***Later interactions will take a duration to complete, for now it just handles them like they're all instantaneous ***
                var text = CurrentInteraction.Target.GetComponent<Interactable>().ExamineText;
                if (text != string.Empty)
                {
                    var point = CurrentInteraction.Target.transform.position;
                }
                break;

            case ActionType.Store:
                if (!inventory.HasItemInHand()) break;
                var item = inventory.Hand1;
                bool itemstored;
                itemstored =
                    CurrentInteraction.Target.GetComponent<Container>().
                        StoreItem(item);
                if (itemstored == true)
                {
                    item.transform.SetParent(CurrentInteraction.Target.transform);
                    item.layer = 2;
                    item.GetComponent<MeshRenderer>().enabled = false;
                    inventory.Clear(item);
                    UpdateInventoryPositions();
                }
                break;

            case ActionType.Open:
                SetState(State.INVENTORY);
                CompleteInteraction();
                break;

            default:
                Debug.Log("Unhandled interaction actiontype " + CurrentInteraction.Type);
                break;
        }
        */
        CurrentInteraction = null;
        SetState(State.IDLE);
    }

    #endregion

    #region Procedural animations

    // Check for a lookat override if there's no interaction queued
    Vector3 GetLookAt(){
        if(CurrentInteraction == null) return PALookAt;
        else return CurrentInteraction.Target.transform.position;
    }

    #endregion

    #region Inventory functions

    // *** This needs to be moved to the inventory component ***
    public void UpdateInventoryPositions()
    {
        // *** There should be a method that can be looped thru for this ***
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
        if (inventory.HasEquipment() == true)
        {
            var invequip = inventory.Equip2;
            var invinter = invequip.GetComponent<Interactable>();
            var rot = invinter.EquipOffsetRot;
            invequip.transform.parent = sheathePosition.transform;
            invequip.transform.localPosition = invinter.EquipOffsetPos;
            invequip.transform.localRotation = new Quaternion();
            invequip.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        UpdateCarryWeights();
    }

    public void UpdateCarryWeights()
    {
        var weight = inventory.CurrentWeight;
        var inhand = inventory.CarriedWeight;

        // *** A Loop here would be really clever ***
        if (inhand <= CarryOneHand) carryState = CarryState.OneHand;
        else if (inhand <= CarryOneHand * 2) carryState = CarryState.TwoHand;
        else carryState = CarryState.Drag;

        if (weight <= MaxCarryWeight / 2) encumberance = Encumberance.Light;
        else if (weight <= MaxCarryWeight) encumberance = Encumberance.Medium;
        else if (weight <= MaxCarryWeight * 1.5) encumberance = Encumberance.Heavy;
        else encumberance = Encumberance.OverEncumber;

        switch (carryState)
        {
            case CarryState.OneHand:
                movement.SetSpeedMultiplier(1);
                break;

            case CarryState.TwoHand:
                movement.SetSpeedMultiplier(1);
                break;

            case CarryState.Drag:
                Debug.Log(carryState);
                movement.SetSpeedMultiplier(.5f);
                break;

            default:
                movement.SetSpeedMultiplier(1);
                break;
        }
        animator.SetCarryWeight((int)carryState);
    }

    public void DropItem(){
        // *** These could be consolidated into one execution ***
        // *** These should also be moved to inventory maybe? ***
        if (inventory.HasItemInHand())
        {
            var handobj = inventory.Hand1;
            var startingrot = handobj.GetComponent<Interactable>().DefaultRotation;
            inventory.Clear(handobj);
            handobj.transform.position = this.transform.position;
            handobj.transform.parent = null;
            handobj.transform.rotation = new Quaternion();
            handobj.transform.localRotation = Quaternion.Euler(startingrot.x, startingrot.y, startingrot.z);
            handobj.layer = 6;
        }
        else if (inventory.HasItemEquipped() || inventory.HasEquipment())
        {
            var obj = inventory.Equip2;
            var startingrot = obj.GetComponent<Interactable>().DefaultRotation;
            inventory.Clear(obj);
            obj.transform.rotation = new Quaternion();
            obj.transform.localRotation = Quaternion.Euler(startingrot.x, startingrot.y, startingrot.z);
            obj.transform.position = this.transform.position;
            obj.transform.parent = null;
            obj.layer = 6;
        }

        UpdateInventoryPositions();
    }

    public void SheatheWeapons(){
        inventory.Swap();
        UpdateInventoryPositions();
    }

    #endregion
}

public enum InteractionPhase {Start, Progress, Complete}