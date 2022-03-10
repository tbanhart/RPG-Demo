using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] public float SpaceMax;

    // *** Leaving this here as a reminder that I might want it eventually ***
    //[SerializeField] public float WeightMax;
    [SerializeField] public List<GameObject> StoredItems;

    public float SpaceLeft {get => SpaceMax - CurrentSpace;}

    public float CurrentSpace{get {
        var space = 0f;
        foreach(var item in StoredItems){
            space += item.GetComponent<Interactable>().Size;
        }
        return space; 
    }}

    public float CurrentWeight {get {
        var weight = 0f;
        foreach(var item in StoredItems){
            weight += item.GetComponent<Interactable>().Weight;
        }
        return weight;
    }}

    public bool StoreItem(GameObject item){
        if(item.GetComponent<Interactable>().Size <= SpaceLeft){
            StoredItems.Add(item);
            return true;
        }
        else return false;   
    }

    public bool TakeItem(GameObject item){
        if(StoredItems.Contains(item)){
            StoredItems.Remove(item);
            return true;
        }
        else return false;
    }
}
