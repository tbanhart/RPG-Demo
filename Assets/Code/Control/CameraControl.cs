using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Vector3 ResetPosition;
    [SerializeField] Vector3 CurrentOffset;

    [SerializeField] float DefaultZoom;
    [SerializeField] float MaxZoom;
    [SerializeField] float MinZoom;
    [SerializeField] float CurrentZoom;

    [SerializeField] Camera CurrentCamera;

    public GameObject CameraTarget;


    public void OffsetRotation(float rotation){
        CameraTarget.transform.RotateAround(CameraTarget.transform.position, new Vector3(0, 1, 0), rotation * Time.deltaTime);
    }   

    public void OffsetZoom(float offset){
        
        var newzoom = CurrentZoom + offset;
        if(newzoom > MaxZoom){
            offset = MaxZoom - CurrentZoom;
        } else if(newzoom < MinZoom){
            offset = MinZoom - CurrentZoom;
        }
        CurrentZoom += offset;
        MoveCamera(CurrentCamera.transform.forward * (offset * Time.deltaTime));
    }

    public void ResetCamera(){
        CurrentCamera.transform.SetPositionAndRotation(ResetPosition, new Quaternion());
        LookAtTarget();
    }

    public void LookAtTarget(){
        CurrentCamera.transform.LookAt(CameraTarget.transform);
    }

    public void MoveCamera(Vector3 magnitude){
        CurrentCamera.transform.position += magnitude;
    }
}
