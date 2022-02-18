using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    Detection detection;

    private void Awake() {
        detection = this.GetComponentInParent<Detection>();
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("target acquired");
        detection.AddTarget(other.gameObject);
    }

    private void OnCollisionExit(Collision other) {
        detection.RemoveTarget(other.gameObject);
    }
}
