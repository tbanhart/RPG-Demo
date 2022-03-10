using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    [SerializeField] GameObject image;

    [SerializeField] public Sprite Image{get => image.GetComponent<Image>().sprite; set => image.GetComponent<Image>().sprite = value;}

    [SerializeField] public GameObject Item;

    GameObject _callingPlayer;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(delegate { GrabItem(); });
    } 

    public void SetupButton(GameObject callingPlayer, Sprite image, GameObject item){
        _callingPlayer = callingPlayer;
        Image = image;
        Item = item;
    }

    public void GrabItem(){
        _callingPlayer.GetComponent<PlayerController>().TryGrabItem(Item);
    }       

}
