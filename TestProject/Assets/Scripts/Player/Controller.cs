using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used to move the player around
public class Controller : MonoBehaviour
{
    //Movement variables
    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput;

    //variable that will turned with the player 
    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    void Start()
    {
        //Checking if you have rigidbody applied
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.Log("you need rigidbody");

        //setting the inputs
        forwardInput = turnInput = 0;
    }
    
    //function that will set the movement keys to (w/a/s/d and up/down/left/right arrows)
    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run();
    }

    //function that checks if key is pressed
    void Run()
    {
        if(Mathf.Abs(forwardInput)>inputDelay)
        {
            //move
            rBody.velocity = transform.forward * forwardInput * forwardVel;
            Debug.Log("SPEEDY");
        }
        else
        {
            //zero velocity
            rBody.velocity = Vector3.zero;
            Debug.Log("NO speedy");
        }
    }

    //function that will turn the player rotation
    void Turn()
    {
        targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;
    }
}
