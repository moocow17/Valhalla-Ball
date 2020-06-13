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
    private float MoveSpeed = 1000f;

    [SerializeField]
    private int playerIndex = 0;

    [SerializeField]
    private float maxThrowForce = 100f;

    [SerializeField]
    private float minThrowForce = 0.2f;

    [SerializeField]
    float maxForceHoldDownTime = 3f;

    private float throwHoldDownStartTime;
    private float holdTimeNormalised;

    private new Rigidbody2D rigidbody2D;
    private new GameObject gameObject;

    private readonly float dropPointDistance = 3.4f;

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
    
    public void SetIsGathering(float buttonPress) //allows player to gather/pickup/steal the ball; actual gathering is done in the IdentifyGather script
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
    
    public void DropBall()//drops the ball behinds the player relative to the direction they are facing and slows its movement while making its movement align with the player movement
    {        
        Collider2D ballsCollider = Helper.FindComponentInChildWithTag<Collider2D>(this.gameObject, "Ball");
        hasBall = false;
        Transform ballObjectTransform = Helper.FindComponentInChildWithTag<Transform>(this.gameObject, "Ball");
        ballsCollider.attachedRigidbody.isKinematic = false;
        ballsCollider.enabled = true;
        ballObjectTransform.position = this.gameObject.transform.position - (this.gameObject.transform.right * dropPointDistance);
        ballObjectTransform.parent = null;
        ballsCollider.attachedRigidbody.velocity = rigidbody2D.velocity * 0.1f;
        isGathering = false;
    }

    public void ChargeThrow()//Gets the time which the player started charging a throw
    {
        throwHoldDownStartTime = Time.time;
    }

    public float CalculateThrowForce(float throwHoldTime)//calculates what the throw force should be based on how long the throw has been charging for
    {     
        holdTimeNormalised = Mathf.Clamp01(throwHoldTime / maxForceHoldDownTime);
        float force = holdTimeNormalised * maxThrowForce;
        force = force + minThrowForce;
        Debug.Log("Throw Hold time: " + throwHoldTime.ToString() + ", Normalised Hold Time: " + holdTimeNormalised + ", Throw force: " + force.ToString());
        return force;
    }

    public void ThrowBall()//throws the ball infront of the player relative to the direction they are facing and changes its velocity based on how long the throw button was held for
    {
        Collider2D ballsCollider = Helper.FindComponentInChildWithTag<Collider2D>(this.gameObject, "Ball");
        hasBall = false;
        Transform ballObjectTransform = Helper.FindComponentInChildWithTag<Transform>(this.gameObject, "Ball");
        ballsCollider.attachedRigidbody.isKinematic = false;
        ballsCollider.enabled = true;
        ballObjectTransform.position = this.gameObject.transform.position + (this.gameObject.transform.right * dropPointDistance);
        ballObjectTransform.parent = null;
        float throwHoldTime = Time.time - throwHoldDownStartTime;
        ballsCollider.attachedRigidbody.velocity = this.gameObject.transform.right * CalculateThrowForce(throwHoldTime);


        /*var speed = ballsCollider.attachedRigidbody.velocity.magnitude;
        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        ballRigidBody.velocity = direction * Mathf.Max(speed, 0f);*/
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
