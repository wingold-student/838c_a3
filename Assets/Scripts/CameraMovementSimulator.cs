using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraMovementSimulator : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed=1000.0f;
    [SerializeField]
    float translationSpeed=1.0f;
    int framesToWaitForMouseLock=30;
    void Awake(){
        //hit ESC to leave the window
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        //this is needed to prevent the huge delta from when your mouse is outside of the window to when it gets locked in the center from making the camera spin around when the lock happens (which can happen within a variable timeframe, so this gives it a few frames to complete the lock)
        //the question mark is a ternary operator, in case you haven't seen it before (it doesn't usually get taught in CS curriculums for some reason). it's basically shorthand for an if statement.
        if (framesToWaitForMouseLock<=0){
            //position needs to be updated based on orientation. Time.deltaTime is used to normalize for framerate
            transform.position +=             
            ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))?transform.forward*translationSpeed*Time.deltaTime:(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))?transform.forward*-translationSpeed*Time.deltaTime:Vector3.zero) +             
            ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))?transform.right*-translationSpeed*Time.deltaTime:(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))?transform.right*translationSpeed*Time.deltaTime:Vector3.zero);
            //yaw rotates around world axis in order to not affect roll
            transform.RotateAround(transform.position,Vector3.up,Input.GetAxis("Mouse X")*Time.deltaTime*rotationSpeed);
            //pitch rotates around local axis
            transform.RotateAround(transform.position,transform.right,-Input.GetAxis("Mouse Y")*Time.deltaTime*rotationSpeed);
            //ensures that roll is never affected (e.g. sometimes Unity gets confused between 0 and 180...causing the camera to turn upside down)
            transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
        } else{
            framesToWaitForMouseLock--;
        }
    }
}
