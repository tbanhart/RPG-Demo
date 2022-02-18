using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHandler
{
    ActorAnimator animator {get;set;}

    Movement movement {get;set;}

    Inventory inventory {get;set;}

    Combat combat {get;set;}

    Interaction CurrentInteraction {get;set;}

    void HandleIdle();

    void HandleMove();

    void HandleAttack();

    void HandleTravelling();

    void HandleInteracting();
}
