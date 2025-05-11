using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    private CharacterMotor motor;
    public Camera cam;
    public LayerMask straightMask;
    public LayerMask curvedMask;

    void Start () {
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update () { 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetAxis("Horizontal") != 0){
                string direction = Input.GetAxis("Horizontal") > 0 ? "right" :"left";
                motor.StartMove(direction, isRunning);
            }
            else if(Input.GetAxis("Vertical") != 0){
                string direction = Input.GetAxis("Vertical") > 0 ? "forward" :"backward";
                motor.StartMove(direction, isRunning);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)){
            motor.StartSquat();
        }
        else if (Input.GetKeyUp(KeyCode.Space)){
            motor.EndSquat();
        }


        if (Input.GetKeyDown(KeyCode.Q)){
            motor.StartSlip('L');
        }
        else if (Input.GetKeyUp(KeyCode.Q)){
            motor.EndSlip();
        }
        else if (Input.GetKeyDown(KeyCode.E)){
            motor.StartSlip('R');
        }
        else if (Input.GetKeyUp(KeyCode.E)){
            motor.EndSlip();
        }

    }
    public void Jab () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, straightMask)){
            motor.StartPunch("1",hit.point);
        }
    }

    public void Straight () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, straightMask)){
            motor.StartPunch("2",hit.point);
        }
    }

    public void LeftUpperCut () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, curvedMask)){
            motor.StartPunch("5",hit.point);
        }
    }

    public void RightUpperCut () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, curvedMask)){
            motor.StartPunch("6",hit.point);
        }
    }

    public void LeftHook () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, curvedMask)){
            motor.StartPunch("3",hit.point);
        }
    }

    public void RightHook () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, curvedMask)){
            motor.StartPunch("4",hit.point);
        }
    }
}
