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

    private new Rigidbody2D rigidbody2D;
    private new GameObject gameObject;

    private readonly float dropPointDistance = 3.2f;

    public Boundary boundary;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;
    private Vector2 aimInputVector = Vector2.zero;
    private Vector2 rigidbody2DPosition = Vector2.zero;

    public bool isGathering = false;
    public bool hasBall = false;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameObject = rigidbody2D.gameObject;
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
        if (buttonPress > 0) //if the button is being pressed down currently it will have a value of 0.001 to 1
        {
            isGathering = true;       
        }
        else
        {
            isGathering = false;
        }
    }
    
    public void DropBall()//drops the ball behinds the player relative to the direction they are facing
    {
        Collider2D ballsCollider = Helper.FindComponentInChildWithTag<Collider2D>(this.gameObject, "Ball");
        hasBall = false;
        Transform ballObjectTransform = Helper.FindComponentInChildWithTag<Transform>(this.gameObject, "Ball");
        ballsCollider.attachedRigidbody.isKinematic = false;
        ballsCollider.enabled = true;
        ballObjectTransform.position = this.gameObject.transform.position - (this.gameObject.transform.right*dropPointDistance); 
        ballObjectTransform.parent = null;
        isGathering = false;

    }

    private void MovePlayer()
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
    }

    private void AimPlayer()
    {
        //set aim direction for player and rotate accordingly
        if (aimInputVector.x + aimInputVector.y != 0)
        {
            aimDirection = new Vector2(aimInputVector.x, aimInputVector.y);
            rigidbody2DPosition = rigidbody2D.transform.position;
            aimDirection = rigidbody2DPosition + aimDirection;
            LookAt2D(rigidbody2D.transform, aimDirection);
        }
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
        AimPlayer();        
    }
}
