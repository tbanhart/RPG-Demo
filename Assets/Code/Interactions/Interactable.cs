using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Interactable : MonoBehaviour
{
    [SerializeField] public List<ActionType> AvailableActions;
    #region Grabbing/Equipping details: See below editor script for these
    [SerializeField] public Vector3 DefaultRotation;
    [HideInInspector] public bool Grabbable;
    [HideInInspector] public bool Equippable;
    [HideInInspector] public bool Equipment;
    [HideInInspector] public Vector3 HandOffsetPos;
    [HideInInspector] public Vector3 HandOffsetRot;
    [HideInInspector] public Vector3 EquipOffsetPos;
    [HideInInspector] public Vector3 EquipOffsetRot;
    #endregion
    [SerializeField] public float Weight;
    [SerializeField] public string ExamineText;

    private void Awake() {
        if(ExamineText == null) ExamineText = string.Empty;
    }

    public Interaction GetInteraction(ActionType action){
        return new Interaction(action, this.gameObject);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Interactable))]
// Source: https://answers.unity.com/questions/1284988/custom-inspector-2.html
public class InteractableGUI : Editor{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Interactable interactable = (Interactable)target;

        interactable.Grabbable = EditorGUILayout.Toggle("Grabbable", interactable.Grabbable);
        if(interactable.Grabbable){
            interactable.HandOffsetPos = EditorGUILayout.Vector3Field("Hand Offset Position",
                interactable.HandOffsetPos);
            interactable.HandOffsetRot = EditorGUILayout.Vector3Field("Hand Offset Rotation",
                interactable.HandOffsetRot);
                
            interactable.Equippable = EditorGUILayout.Toggle("Equippable", interactable.Equippable);
            if (interactable.Equippable)
            {
                interactable.Equipment = EditorGUILayout.Toggle("Equipment", 
                    interactable.Equipment);
                interactable.EquipOffsetPos = EditorGUILayout.Vector3Field("Equip Offset Position",
                    interactable.EquipOffsetPos);
                interactable.EquipOffsetRot = EditorGUILayout.Vector3Field("Equip Offset Rotation",
                    interactable.EquipOffsetRot);
            }
        }

    }
}
#endif