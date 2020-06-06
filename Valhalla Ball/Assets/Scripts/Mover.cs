using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}


public class Mover : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 600;

    [SerializeField]
    private int playerIndex = 0;

    //private CharacterController controller;
    private new Rigidbody2D rigidbody2D;

    public Boundary boundary;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;
    private Vector2 rigidbody2DPosition = Vector2.zero;
    
    private void Awake()
    {
        //controller = GetComponent<CharacterController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }


    public void SetMoveInputVector(Vector2 direction)
    {
        moveInputVector = direction;
    }

    public void SetAimInputVector(Vector2 direction)
    {
        aimInputVector = direction;
    }

    public static void LookAt2D(Transform transform, Vector2 target)
    {
        Vector2 current = transform.position;
        var direction = target - current;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    
    void FixedUpdate()
    {
        moveDirection = new Vector2(moveInputVector.x, moveInputVector.y);
        moveDirection = moveDirection * MoveSpeed * Time.fixedDeltaTime;
        rigidbody2D.velocity = moveDirection;
        //rigidbody2D.MovePosition(rigidbody2D.position + moveDirection);
        rigidbody2D.position = new Vector3
        (
            Mathf.Clamp(rigidbody2D.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rigidbody2D.position.y, boundary.yMin, boundary.yMax)
        );

        if (aimInputVector.x + aimInputVector.y != 0)
        {
            aimDirection = new Vector2(aimInputVector.x, aimInputVector.y);
            rigidbody2DPosition = rigidbody2D.transform.position;
            aimDirection = rigidbody2DPosition + aimDirection;
            LookAt2D(rigidbody2D.transform, aimDirection);
        }
            


        // Debug.Log("aimInputVector: " + aimInputVector.ToString());
        //aimDirection = new Vector3(aimInputVector.x, aimInputVector.y);
        //Debug.Log("aimDirection: " + aimDirection.ToString());
        // Debug.Log("rigidbody2D.transform.position: " + rigidbody2D.transform.position + "aimDirection: " + aimDirection);
        //rigidbody2D.transform.LookAt(rigidbody2D.transform.position + aimDirection);

        //rigidbody2D.transform.LookAt(lookPoint);

        /*START NON CODE
        float moveHorizontal = Input.GetAxis("Horizontal"); //gets the X input from the joystick; this resets to 0 when joystick is neutral; 1 is the max value
        float moveVertical = Input.GetAxis("Vertical"); //gets the Y input from the joystick; this resets to 0 when joystick is neutral; 1 is the max value

        if (moveHorizontal + moveVertical != 0)
        {
            moveHorizontalRotation = moveHorizontal;
            moveVerticalRotation = moveVertical;
        }

        Vector3 rotation = new Vector3(moveHorizontalRotation, 0.0f, moveVerticalRotation);

        //Makes it rotate to the direction being moved in
        GetComponent<Rigidbody>().rotation = (Quaternion.LookRotation(rotation));
        //END NON CODE */
    }
}
