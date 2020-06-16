using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Mover mover;
    private int pressRBCounter = 0;
    private int pressLBCounter = 0;
    private bool rightTriggerAlreadySuppressed = false;
    private bool leftTriggerAlreadySuppressed = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var movers = FindObjectsOfType<Mover>(); 
        var index = playerInput.playerIndex;
        mover = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }

    public void OnMove(CallbackContext context)
    {
        if(mover != null)
        {
            mover.SetMoveInputVector(context.ReadValue<Vector2>());
        }        
    }

    public void OnAim(CallbackContext context)
    {
        if (mover != null)
        {
            mover.SetAimInputVector(context.ReadValue<Vector2>());
        }
    }

    public void OnLBumper(CallbackContext context) //this is coded a bit shit; currently it is forced for Drop Ball and Gather Ball to be mapped to the same button, potentially move sorting logic into Mover class or another class
    {
        pressLBCounter++;

        //Debug.Log("LB presses: " + pressCounterLB.ToString());
        if (mover != null)
        {
            if (pressLBCounter == 1) //onEntered: when button is first pressed
            {
                //do nothing?               
            }
            if (pressLBCounter == 2) //onPressed: when button is first pressed but after onEntered; if you hold down button it wont do anything further until released
            {
                if (mover.hasBall == true) //if they DO have the ball then release it
                {
                    mover.DropBall();
                }
                else //if they DONT have the ball then set isGathering to true; the gatherCollider will then automatically pick it up
                {
                    mover.SetIsGathering(context.ReadValue<float>());
                }                
            }
            if (pressLBCounter == 3) //onRelease: when button is released
            {
                //if they DONT have the ball then set isGathering to false
                mover.SetIsGathering(context.ReadValue<float>());
                pressLBCounter = 0; //on release prep it so the next press takes them to onEntered again
            }     
        }
    }

    public void OnRBumper(CallbackContext context) //this is coded a bit shit; currently it is forced for Drop Ball and Gather Ball to be mapped to the same button, potentially move sorting logic into Mover class or another class
    {
        pressRBCounter++;

        //Debug.Log("LB presses: " + pressCounterLB.ToString());
        if (mover != null)
        {
            if (pressRBCounter == 1) //onEntered: when button is first pressed
            {
                mover.Attack();//
            }
            if (pressRBCounter == 2) //onPressed: when button is first pressed but after onEntered; if you hold down button it wont do anything further until released
            {
                //
            }
            if (pressRBCounter == 3) //onRelease: when button is released
            {
                //
                pressRBCounter = 0; //on release prep it so the next press takes them to onEntered again
            }
        }
    }

    public void OnRTrigger(CallbackContext context)
    {
        if (mover != null)
        {
            if (context.ReadValue<float>() > 0f) //onPressed (this could run an unknown amount of times)
            {
                
                if (rightTriggerAlreadySuppressed == false) //used to make sure the action only runs once until this trigger is released
                {
                    if (mover.hasBall == true) //if they DO have the ball then start charging
                    {
                        mover.ChargeThrow();//start charging
                    }
                    else //if they DONT have the ball then do nothing? (potentially make this shield)
                    {

                    }
                }
                rightTriggerAlreadySuppressed = true;
            }
            else if (context.ReadValue<float>() == 0f) //onRelease
            {
                if (mover.hasBall == true) //if they DO have the ball then release the throw
                {
                    mover.ThrowBall();
                }
                else //if they DONT have the ball then do nothing? need to factor in the possibility that the ball has been stolen since they started charging
                {

                }
                rightTriggerAlreadySuppressed = false;
            }
        } 
    }

    public void OnLTrigger(CallbackContext context)
    {
        if (mover != null)
        {
            if (context.ReadValue<float>() > 0f) //onPressed (this could run an unknown amount of times)
            {

                if (leftTriggerAlreadySuppressed == false) //used to make sure the action only runs once until this trigger is released
                {
                    
                }
                leftTriggerAlreadySuppressed = true;
            }
            else if (context.ReadValue<float>() == 0f) //onRelease
            {
                
                leftTriggerAlreadySuppressed = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
