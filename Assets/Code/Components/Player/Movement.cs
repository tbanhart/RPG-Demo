using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    #region public Properties

    [SerializeField] public float MoveSpeed;

    public Vector3 Destination{get => agent.destination;}

    public bool IsMoving {get{
        if(agent.remainingDistance <= agent.stoppingDistance){
            return false;
        }
        else return true;
    }}
    
    #endregion

    #region Unity Defaults

    private void Awake() {
        SetSpeed(MoveSpeed);
    }

    #endregion

    #region private Properties

    NavMeshAgent agent {get => this.GetComponent<NavMeshAgent>();}

    #endregion

    #region Navmesh pass functions

    public void SetDestination(Vector3 destination){
        agent.destination = destination;
    }

    public void CancelMovement(){
        agent.ResetPath();
    }

    public void SetSpeed(float speed){
        agent.speed = speed;
    }

    public Vector3 GetVelocity()
    {
        /*
        var vec = this.transform.position -  agent.nextPosition;
        Debug.Log(vec);     
        return vec;
        */

        Vector3 normalizedMovement = agent.desiredVelocity.normalized;

        Vector3 forwardVector = Vector3.Project(normalizedMovement, transform.forward);

        Vector3 rightVector = Vector3.Project(normalizedMovement, transform.right);

        // Dot(direction1, direction2) = 1 if they are in the same direction, -1 if they are opposite

        float forwardVelocity = forwardVector.magnitude * Vector3.Dot(forwardVector, transform.forward);

        float rightVelocity = rightVector.magnitude * Vector3.Dot(rightVector, transform.right);

        return new Vector3(rightVelocity * MoveSpeed, 0, forwardVelocity * MoveSpeed);
    }

    internal void SetStoppingDistance(float distance)
    {
        agent.stoppingDistance = distance;
    }

    public float GetDistance(Vector3 position){
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(position, path);
        var corners = path.corners;
        float distance = 0f;

        for(var corner = 0; corner < corners.Length - 1; corner++){
            distance += Vector3.Distance(corners[corner], corners[corner+1]);
        }
        
        return distance;
    }

    #endregion


}
