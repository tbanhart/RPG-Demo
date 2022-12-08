using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public List<GameObject> Targets;

    public GameObject Aggro;

    [SerializeField] Collider detcollider;

    [SerializeField] public float Radius;

    private void Awake() {
        Targets = new List<GameObject>();
        Aggro = null;
    }

    public bool HasAggro(){
        if(Aggro == null) return false;
        else return true;
    }

    public void AddTarget(GameObject obj){
        Targets.Add(obj);
        GameObject target;
        if((target = Targets.Find(t => t.name == "Player")) != null){
            Aggro = target;
        }
    }

    public void RemoveTarget(GameObject obj){
        Targets.Remove(obj);
    }

    public List<GameObject> GetTargets(bool awareness){
        return Targets;
    }
}


