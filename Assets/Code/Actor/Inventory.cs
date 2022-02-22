using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // I'm not sure if this will last, placeholder for the slots
    //  starting with just the hands and equipment
    public enum InventorySlot{HAND1, EQUIP1}

    //public GameObject Hand1 {get => Items[(int)InventorySlot.HAND1].GetItem();}
    //public GameObject Equip1 { get => Items[(int)InventorySlot.EQUIP1].GetItem();}

    // *** The array method wasn't working
    //InventoryItem _hand1 {get => Items[(int)InventorySlot.HAND1]; set => Items[(int)InventorySlot.HAND1] = value;}
    //InventoryItem _equip1 {get => Items[(int)InventorySlot.EQUIP1]; set => Items[(int)InventorySlot.EQUIP1] = value;}

    #region Inventory Slot Properties

    // ***This will be way less painful to add things to if I create an object that has the following 
    //  info in it***

    // Locations for where objects go when equipped or held
    [SerializeField] public GameObject HandLocation;
    [SerializeField] public GameObject EquipLocation;

    // Accessors for inventory slots
    public GameObject Hand1 {get => _hand1.GetItem();}
    public GameObject Equip1 {get => _equip1.GetItem();}

    InventoryItem _hand1 {get; set;}
    InventoryItem _equip1 {get; set;}

    #endregion

    // It made sense to make this a class to read the inv item as a gameobject or reference to data
    List<InventoryItem> Items;

    private void Awake() {
        // Initialize inventory
        Items = new List<InventoryItem>(2);
        _hand1 = null;
        //Items[(int)InventorySlot.HAND2] = null;
        _equip1 = null;
        //Items[(int)InventorySlot.EQUIP2] = null;
        //Items[(int)InventorySlot.EQUIP3] = null;
    }

    public bool CanPickup(GameObject obj){
        if(_hand1 == null){
            Debug.Log("Hand empty");
            return true;
        } else if (_equip1 == null
         && _hand1.Equippable == true){
            return true;
        } else{ 
            return false;
        }
    }

    public bool Pickup(GameObject obj){
        var invobj = new InventoryItem(obj);
        Debug.Log("Running pickup script");
        if(CanPickup(obj) == true){
            // Pick up if hands are free
            if(_hand1 == null){
                _hand1 = invobj;
                Debug.Log("Picked up " + obj.name);
                return true;
            } 

            // Try to equip what is in hands
            else if(_hand1.Equippable == true
             && _equip1 == null){
                // Equip and pick it up
                Swap();
                _hand1 = invobj;
                Debug.Log("Picked up " + obj.name);
                return true;
            }

            // Cancel action if not possible
            else{
                Debug.Log("Item can be picked up but no space");

                return false;
            }
        }
        Debug.Log("Unable to pick up item :(");

        return false;
    }

    // Swap out hand and equip
    public bool Swap(){
        InventoryItem item;
        var hand1 = _hand1;
        // Try to equip from slot 1
        if(_equip1 != null){

            // Equip if hand 1 is empty
            if(_hand1 == null){
                Debug.Log("Equipped " + Equip1.name);
                _hand1 = _equip1;
                _equip1 = null;
                return true;
            }

            // If hand1 is equippable swap with equip1
            else if(_hand1.Equippable == true){
                Debug.Log("Swapped " + Equip1.name + " and " + Hand1.name);
                item = _equip1;
                _equip1 = _hand1;
                _hand1 = item;
                return true;
            }

            // Return that item can't be swapped
            else {
                Debug.Log("Can't equip, weapon in hands isn't equippable");
                return false;
            }
        }

        // Try to equip the hand item if nothing is equipped
        else{

            // If nothing is in hands, return that swap isn't available
            if(_hand1 == null){
                Debug.Log("Couldn't swap, nothing in equipment and nothing in hands.");
                return false;
            }

            // Try to equip what is in hands
            else if(_hand1.Equippable == true){
                Debug.Log("Equipped item from hands");
                _equip1 = _hand1;
                _hand1 = null;
                return true;
            }

            // Return, item can't be swapped
            else{
                Debug.Log("Item in hands can't be swapped");
                return false;
            }
        }
    }

    // Clears designated object out of inventory
    public void Clear(GameObject obj){
        if(_hand1 != null && _hand1.GetItem() == obj) _hand1 = null;
        if(_equip1 != null && _equip1.GetItem() == obj) _equip1 = null;
    }

    public bool HasItemInHand(){
        if(_hand1 == null){
            return false;
        } else return true;
    }

    public bool HasItemEquipped(){
        if(_equip1 == null){
            return false;
        } else return true;
    }
}

public class InventoryItem
{
    GameObject Item;

    public bool Equippable;

    float Weight;

    public InventoryItem(GameObject item){
        Item = item;
        Equippable = item.GetComponent<Interactable>().Equippable;
        Weight = item.GetComponent<Interactable>().Weight;
    }

    public GameObject GetItem()
    {
        return Item;
    }
}

