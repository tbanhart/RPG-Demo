using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Movement movement;

    Combat combat;

    ConsumableHandler _handler;

    Inventory inventory;

    ActorAnimator animator;

    Interaction CurrentInteraction;

    GameObject _target;

    [SerializeField] State CurrentState;

    Detection detection;

    float ChaseTimer;

    [SerializeField] Vector3 Origin;

    [SerializeField] float MaxChase;

    private void Awake() {
        _handler = new EnemyHandler(this.gameObject, null, null);

        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<ActorAnimator>();
        detection = GetComponent<Detection>();
        CurrentInteraction = null;
        SetState(State.IDLE);
        MaxChase /= Time.deltaTime;
        Origin = this.transform.position;
        _target = GameObject.FindGameObjectWithTag("Player");

        CurrentState = State.IDLE;
    }

    private void Update() {
        switch(CurrentState){
            case State.IDLE:
                if(movement.GetDistance(_target.transform.position) < detection.Radius)
                {
                    _handler.SetState(State.TRAVELLING);
                    _handler.currentInteraction = new Interaction(ActionType.Walk, _target, 3f);
                }
                break;

            case State.TRAVELLING:
                
                break;

            case State.MOVING:
                if(movement.IsMoving == false){
                    
                    SetState(State.IDLE);
                    animator.SetMovement(Vector3.zero);
                } else animator.SetMovement(movement.GetVelocity());
                break;
            
            case State.ATTACKING:
                Debug.Log("Attacking");
                // Needs rework - uses old system
                // Deal damage and return if the target is dead
                /*
                var isDead = combat.DoAttack(CurrentInteraction.Target.GetComponent<Combat>());
                animator.SetAttacking(true);
                if (isDead == true)
                {
                    CurrentInteraction.Target.GetComponent<Combat>().Kill(this.name);
                    CurrentInteraction = null;
                    SetState(State.IDLE);
                    animator.SetAttacking(false);
                }
                break;
                */
                break;
        }
    }

    void SetState(State state){
        CurrentState = state;
    }

}
