using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public ActionType Type {get; set;}

    public GameObject Target {get; set;}

    public Interaction(ActionType type, GameObject target){
        Type = type;
        Target = target;
    }
}
