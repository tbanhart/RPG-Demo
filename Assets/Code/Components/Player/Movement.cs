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

    #endregion


}
