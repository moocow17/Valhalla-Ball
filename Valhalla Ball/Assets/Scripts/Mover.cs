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

public enum AttackState
{
    Idle,
    Windup,
    Backswing
}

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float moveSpeed;

    public int playerIndex = 0;
    public int playerTeam = 0;

    [SerializeField]
    private float maxThrowForce;

    [SerializeField]
    private float minThrowForce;

    [SerializeField]
    float maxForceHoldDownTime;

    private float throwHoldDownStartTime;
    private float holdTimeNormalised;

    [SerializeField]
    private float boostStrength;

    [SerializeField]
    private float boostCapacity;

    [SerializeField]
    private float maxBoostCapacity;

    public bool isBoosting;


    float hitStartupTime;
    [SerializeField]
    float hitStartupTimeIncrement;

    public AttackState attackState;
    public float attackStateTime;
    //public bool isSwinging = false;

    float hitFreezeTime;
    [SerializeField]
    float hitFreezeTimeIncrement;
    //public bool isFrozen = false;
    
    float nextAttackTime;
    [SerializeField]
    float nextAttackTimeIncrement;
    public bool isHitting = false;

    private RespawnManager gameController;
    private new Rigidbody2D rigidbody2D;
    private new GameObject gameObject;

    private readonly float dropPointDistance = 3.4f;
    public float hitStrengthMultiplier = 5000f;

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
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<RespawnManager>();
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

    public static void LookAt2D(Transform transform, Vector2 target) //NOT USED CURRENTLY; IF ROTATION SPEED IS LIKED THEN CAN REMOVE
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
    }

    public void AttemptHit()
    {
        if (attackState == AttackState.Idle && Time.time >= attackStateTime + nextAttackTimeIncrement)
        {
            Debug.Log("State: " + attackState.ToString());
            attackState = AttackState.Windup;
            attackStateTime  = Time.time;
            Debug.Log("Transitioned to: " + attackState.ToString());
        }
    }

    void Attack()//hits in front of the player; any other players in the colliders in the Hitbox gameobject will die
    {
        Debug.Log("1");
        Collider2D currentPlayerPolygonCollider = gameObject.FindComponentInChildWithTag<PolygonCollider2D>("Hitbox");
        Debug.Log("2");
        Collider2D currentPlayerCircleCollider = gameObject.FindComponentInChildWithTag<CircleCollider2D>("Hitbox");
        Debug.Log("3");

        //ATTACK ANIMATION/EFFECT

        //DETECT ALL OTHER PLAYERS THAT ARE HIT   
        //get player colliders hit by the polygon hit collider stored as a list        
        List<Collider2D> hitPlayersFromPolygonCollider = new List<Collider2D>();
        LayerMask playerLayerMask = LayerMask.GetMask("Player");
        ContactFilter2D playerContactFilter = new ContactFilter2D();
        playerContactFilter.SetLayerMask(playerLayerMask);//With this, the overlap collider will only detect colliders that have the "Player" layer on it, including the controlled player
        int polygonColliderCount = currentPlayerPolygonCollider.OverlapCollider(playerContactFilter, hitPlayersFromPolygonCollider);

        //get player colliders hit by the circle hit collider stored as a list
        List<Collider2D> hitPlayersFromCircleCollider = new List<Collider2D>();
        int circleColliderCount = currentPlayerCircleCollider.OverlapCollider(playerContactFilter, hitPlayersFromCircleCollider);

        //get union of the above lists
        IEnumerable<Collider2D> unionOfHitBoxLists = hitPlayersFromPolygonCollider.Union(hitPlayersFromCircleCollider);

        //KILL THE HIT PLAYERS (except the player that did the hit)
        foreach (Collider2D playerCollider in unionOfHitBoxLists)
        {
            //disattach balls from players who are about to die
            Collider2D ballsCollider = Helper.FindComponentInChildWithTag<Collider2D>(playerCollider.gameObject, "Ball");
            if (ballsCollider != null)
            {
                ballsCollider.attachedRigidbody.isKinematic = false;
                ballsCollider.enabled = true;
                ballsCollider.transform.parent = null;
                Mover playerMover = (Mover)playerCollider.gameObject.GetComponent(typeof(Mover));
                playerMover.hasBall = false;
            }
            //kill the players    
            if (playerCollider.name != this.gameObject.name)
            {
                /*Vector2 direction = (playerCollider.transform.position - transform.position)*10000; //intended to shoot the player off the edge really fast like in advance wars so they could explode off the edge like in smash bros; couldnt get it to work
                playerCollider.attachedRigidbody.AddForceAtPosition(direction, transform.position);*/
                KillPlayer(playerCollider.gameObject);
            }
        }

        //DEATH ANIMATIONS add: https://www.youtube.com/watch?v=uR2jcU3x3kU

        //GET ALL BALLS THAT WILL BE HIT
        //get ball colliders hit by the polygon hit collider stored as a list
        List<Collider2D> hitBallsFromPolygonCollider = new List<Collider2D>();
        LayerMask ballLayerMask = LayerMask.GetMask("Ball");
        ContactFilter2D ballContactFilter = new ContactFilter2D();
        ballContactFilter.SetLayerMask(ballLayerMask);//With this, the overlap collider will only detect colliders that have the "Ball" layer on it
        int polygonBallColliderCount = currentPlayerPolygonCollider.OverlapCollider(ballContactFilter, hitBallsFromPolygonCollider);

        //get player colliders hit by the circle hit collider stored as a list
        List<Collider2D> hitBallsFromCircleCollider = new List<Collider2D>();
        int circleBallColliderCount = currentPlayerCircleCollider.OverlapCollider(ballContactFilter, hitBallsFromCircleCollider);

        //get union of the above lists
        IEnumerable<Collider2D> unionOfBallHitBoxLists = hitBallsFromPolygonCollider.Union(hitBallsFromCircleCollider);

        //SEND FORCES OUTWARDS TO AFFECT BALL  
        foreach (Collider2D ballCollider in unionOfBallHitBoxLists)
        {
            Vector2 direction = (ballCollider.transform.position - currentPlayerCircleCollider.transform.position);
            Debug.Log("Direction: " + direction.ToString());
            Debug.Log("Direction normalised: " + direction.normalized.ToString());
            ballCollider.attachedRigidbody.AddForce(direction.normalized * hitStrengthMultiplier);
        }

        //COMMENCE ATTACK FREEZE - PREVENTS PLAYER FROM DOING ANYTHING ELSE UNTIL FREEZETIME IS OVER
        hitFreezeTime = Time.time + hitFreezeTimeIncrement;
        
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        gameController.PrepPlayerRespawn(player);
    }

    private void MovePlayer()
    {
        //move player in direction     
        moveDirection = new Vector2(moveInputVector.x, moveInputVector.y);
        Vector2 boostDirection = transform.right;
        if (isBoosting == true && boostCapacity > 0)
        {
            moveDirection = boostDirection * boostStrength * moveSpeed * Time.fixedDeltaTime;
            boostCapacity -= Time.fixedDeltaTime;
        }
        else
        {
            moveDirection = moveDirection * moveSpeed * Time.fixedDeltaTime;
        }
        
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
            //LookAt2D(rigidbody2D.transform, aimDirection);

            Vector2 dir = aimDirection - rigidbody2DPosition;
            float angle = Vector2.Angle(transform.right, dir);
            if (angle > 5f)
            {
                float rDir = Mathf.Sign(Vector2.SignedAngle(transform.right, dir));
                transform.Rotate(Vector3.forward * Time.deltaTime * rDir * rotateSpeed);
            }
        }
    }

    private void Update()
    {
        if (attackState == AttackState.Windup && Time.time >= attackStateTime + hitStartupTimeIncrement) {
            Debug.Log("State: " + attackState.ToString());
            Debug.Log("Attack!");
            Attack();
            attackState = AttackState.Backswing;
            attackStateTime = Time.time;
            Debug.Log("Transitioned to: " + attackState.ToString());
        }

        if (attackState == AttackState.Backswing && Time.time >= attackStateTime + hitFreezeTimeIncrement) {
            Debug.Log("State: " + attackState.ToString());
            attackState = AttackState.Idle;
            attackStateTime = Time.time;
            Debug.Log("Transitioned to: " + attackState.ToString());
        }   
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
        if (attackState != AttackState.Backswing)
        {
            MovePlayer();
            AimPlayer();
        }

        if (!isBoosting)
        {
            if (boostCapacity <= maxBoostCapacity)
            {
                boostCapacity += Time.fixedDeltaTime;
            }                
        }

        if (boostCapacity <= maxBoostCapacity)        
            Debug.Log(boostCapacity.ToString());
    }
}
