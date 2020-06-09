using System;
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
    private float MoveSpeed = 1000;

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

    public void GatherBall(float buttonPress)
    {
        //FUCK, JUST REALISED THIS APPROACH WONT WORK, IGNORE BELOW
        //ACTIVATES COLLIDER WHICH WHEN IT COMES INTO CONTACT WITH BALL OR CARRIER WILL GIVE BALL TO THIS PLAYER
        //set a bool 'isGathering' to true if button is being pressed; 
        //set it to false if not being pressed (might have to do this in Update() function as this method will only be called if it is pressed
        /*in the colliders method: 
         * check if collider is hitting ball, if so and if player's isGathering is true:
         * {
         *  move ball instance to centre of player and attach it so it follows
         *  set a variable in the player to indicate they own the ball (is this a race condition if two players grab ball?)
         *  change the same variable in all other players to indicate they no longer own the ball
         * }
         * else:
         * {
         *  if isGathering is true: loop through other players and if connecting with another player who has the ball do same as above
         * } */
    }

    void FixedUpdate()
    {
        //move player in direction
        moveDirection = new Vector2(moveInputVector.x, moveInputVector.y);
        moveDirection = moveDirection * MoveSpeed * Time.fixedDeltaTime;
        rigidbody2D.velocity = moveDirection;
        rigidbody2D.position = new Vector3
        (
            Mathf.Clamp(rigidbody2D.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rigidbody2D.position.y, boundary.yMin, boundary.yMax)
        );

        //set aim direction for player and rotate accordingly
        if (aimInputVector.x + aimInputVector.y != 0)
        {
            aimDirection = new Vector2(aimInputVector.x, aimInputVector.y);
            rigidbody2DPosition = rigidbody2D.transform.position;
            aimDirection = rigidbody2DPosition + aimDirection;
            LookAt2D(rigidbody2D.transform, aimDirection);
        }
    }
}
