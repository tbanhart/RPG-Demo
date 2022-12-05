using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();   
    }

    public void SetMovement(Vector3 velocity)
    {
        ClearFlags();
        if(velocity.x + velocity.z != 0){
            animator.SetBool("IsMoving", true);
            animator.SetFloat("XVelocity", velocity.x);
            animator.SetFloat("YVelocity", velocity.z);
        }        
        else{
            animator.SetBool("IsMoving", false);
            animator.SetFloat("XVelocity", velocity.x);
            animator.SetFloat("YVelocity", velocity.z);
        }
    }

    public void SetAttacking(bool isAttacking){
        //SetMovement(Vector3.zero);

        //animator.SetBool("IsMoving", false);
        //animator.SetBool("IsAttacking", isAttacking);
        animator.SetTrigger("TriggerAttack");
    }

    public void ClearFlags(){
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", false);
    }

    public void SetCarryWeight(int state){
        animator.SetFloat("CarryState", state);
    }
}
