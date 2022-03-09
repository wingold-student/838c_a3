using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StereoCamera : MonoBehaviour
{
    private GameObject leftEyeCamera,rightEyeCamera;
    public float eyeSeparationIPD = 0.08f;
    public float nearClipPlane = 0.01f;
    public float farClipPlane = 100.0f;
    public float fieldOfView = 60.0f;
    public float aspectMultiplier=1.0f;
    public float screenBorderWidthX=0.01f,screenBorderWidthY=0.01f;
    
    void Awake()
    {
        this.gameObject.GetComponent<Camera>().enabled=false;
        leftEyeCamera = new GameObject("leftEyeCamera");
        leftEyeCamera.transform.SetParent(gameObject.transform.transform);
        leftEyeCamera.transform.localPosition=new Vector3(0,0,0);
        leftEyeCamera.transform.localPosition -= new Vector3(eyeSeparationIPD * 0.5f, 0, 0);
        var cameraLE = leftEyeCamera.AddComponent<Camera>();
        cameraLE.rect = new Rect(screenBorderWidthX, screenBorderWidthY, 0.5f-(2.0f*screenBorderWidthX), 1.0f-(2.0f*screenBorderWidthY));
        cameraLE.fieldOfView = fieldOfView;
        cameraLE.aspect *= aspectMultiplier;
        cameraLE.nearClipPlane = nearClipPlane;
        cameraLE.farClipPlane = farClipPlane;
        leftEyeCamera.SetActive(true);
 
        rightEyeCamera = new GameObject("rightEyeCamera");
        rightEyeCamera.transform.SetParent(gameObject.transform.transform);
        rightEyeCamera.transform.localPosition=new Vector3(0,0,0);
        rightEyeCamera.transform.localPosition += new Vector3(eyeSeparationIPD * 0.5f, 0, 0);
        var cameraRE = rightEyeCamera.AddComponent<Camera>();
        cameraRE.rect = new Rect(0.5f+screenBorderWidthX, screenBorderWidthY, 0.5f-(2.0f*screenBorderWidthX), 1.0f-(2.0f*screenBorderWidthY));
        cameraRE.fieldOfView = fieldOfView;
        cameraRE.aspect *= aspectMultiplier;
        cameraRE.nearClipPlane = nearClipPlane;
        cameraRE.farClipPlane = farClipPlane;
        rightEyeCamera.SetActive(true);
 
    }
}

