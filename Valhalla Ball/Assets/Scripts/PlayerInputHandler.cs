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
            Debug.Log(context.valueType.ToString());
        }
    }

    public void OnLBumper(CallbackContext context)
    {
        if (mover != null)
        {
            mover.GatherBall(context.ReadValue<float>());
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
