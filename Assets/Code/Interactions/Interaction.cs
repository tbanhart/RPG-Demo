using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public ActionType Type {get; set;}

    public GameObject Target {get; set;}

    public float Distance {get; set;}

    public float Effect {get; set;}

    public bool IsComplete {get; set;}

    public Interaction(ActionType type, GameObject target, float distance = 0f, float effect = 0f){
        Type = type;
        Target = target;
        Distance = distance;
        Effect = effect;
        
        IsComplete = false;
    }

    public Interaction(ActionType type, GameObject target, float distance){
        Type = type;
        Target = target;
        Distance = distance;
    }
}
