using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public ActionType Type {get; set;}

    public GameObject Target {get; set;}

    public float Distance {get; set;}

    public float Effect {get; set;}

    public float Progress{get; set;}

    public bool IsComplete {get; set;}

    public InteractionPhase Phase {get; set;}

    public Interaction(ActionType type, GameObject target, float distance = 3f, float effect = 0f, float duration = 1f){
        Type = type;
        Target = target;
        Distance = distance;
        Effect = effect;
        Progress = duration;

        IsComplete = false;

        Phase = InteractionPhase.Start;
    }
}
