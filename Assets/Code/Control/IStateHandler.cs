using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHandler
{
    ActorAnimator animator {get;set;}

    Movement movement {get;set;}

    Inventory inventory {get;set;}

    Combat combat {get;set;}

    Interaction currentInteraction {get;set;}

    Vector3 selectorPosition {get; set;}

    State currentState {get; set;}

    void HandleState();

    void HandleIdle();

    void HandleMove();

    void HandleAttack();

    void HandleTravelling();

    void HandleInteracting();

    void SetState(State state);

    void SetState(ActionType action);
}
