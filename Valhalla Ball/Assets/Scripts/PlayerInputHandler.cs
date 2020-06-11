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
        if (mover != null)
        {
            if (mover.hasBall == true)
            {
                mover.DropBall();
            }
            else
            {
                mover.GatherBall(context.ReadValue<float>());
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
