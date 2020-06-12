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
    private int pressCounter = 0;

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
        pressCounter++;
        if (mover != null)
        {
            if (pressCounter == 1) //onEntered: when button is first pressed
            {
                //do nothing?               
            }
            if (pressCounter == 2) //onPressed: when button is first pressed but after onEntered; if you hold down button it wont do anything further until released
            {

                if (mover.hasBall == true) //if they DO have the ball then release it
                {
                    mover.DropBall();
                }
                else //if they DONT have the ball then set isGathering to true; the gatherCollider will then automatically pick it up (maybe get GatherCollider to set isGathering to false once gathered?)
                {
                    mover.SetIsGathering(context.ReadValue<float>());
                }
                
            }
            if (pressCounter == 3) //onRelease: when button is released
            {
                //if they DONT have the ball then set isGathering to false
                mover.SetIsGathering(context.ReadValue<float>());
                pressCounter = 0; //on release prep it so the next press takes them to onEntered again
            }     
        }
    }

    public void OnRTrigger(CallbackContext context)
    {
        if (mover != null)
        {
            
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
