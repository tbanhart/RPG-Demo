using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();   
    }

    public void SetMovement(Vector3 velocity)
    {
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
}
