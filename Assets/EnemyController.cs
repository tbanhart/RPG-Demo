using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Movement movement;

    Combat combat;

    Inventory inventory;

    PlayerAnimator animator;

    Interaction CurrentInteraction;

    [SerializeField] State CurrentState;

    Detection detection;

    float ChaseTimer;

    [SerializeField] Vector3 Origin;

    [SerializeField] float MaxChase;

    private void Awake() {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<PlayerAnimator>();
        detection = GetComponent<Detection>();
        CurrentInteraction = null;
        SetState(State.IDLE);
        MaxChase /= Time.deltaTime;
        Origin = this.transform.position;
    }

    private void Update() {
        switch(CurrentState){
            case State.IDLE:
                if(detection.HasAggro() == true){
                    CurrentInteraction = new Interaction(
                        ActionType.Attack,
                        detection.Aggro
                    );
                    SetState(State.TRAVELLING);
                    ChaseTimer = 0f;
                    movement.SetDestination(detection.Aggro);
                }
                break;

            case State.TRAVELLING:
                if(movement.GetDistance(detection.Aggro) <= combat.Range){
                    animator.SetMovement(Vector3.zero);
                    SetState(State.ATTACKING);
                } else{
                    ChaseTimer += 1f;
                    if (ChaseTimer >= MaxChase){
                        SetState(State.MOVING);
                        movement.SetDestination(Origin);
                    }
                    animator.SetMovement(movement.GetVelocity());
                }
                break;

            case State.MOVING:
                if(movement.IsMoving == false){
                    
                    SetState(State.IDLE);
                    animator.SetMovement(Vector3.zero);
                } else animator.SetMovement(movement.GetVelocity());
                break;
            
            case State.ATTACKING:
                // Deal damage and return if the target is dead
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
        }
    }

    void SetState(State state){
        CurrentState = state;
    }

}
