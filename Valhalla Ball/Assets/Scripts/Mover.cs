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

public enum ActionState
{
    Idle,
    AttackWindup,
    AttackBackswing,
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

    public GameObject preHitChargePrefab;
    GameObject preHitParticles;
    public GameObject hitPrefab;
    GameObject hitParticles;
    public GameObject deathParticlesPrefab;
    GameObject deathParticles;



    [SerializeField]
    private float hitPointDistance = 2.0f;
    [SerializeField]
    private float preHitPointDistance = 3.0f;

    float hitStartupTime;
    [SerializeField]
    float hitStartupTimeIncrement;

    public ActionState actionState;
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
    public bool hasChargedThrow = false;

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
            if (actionState == ActionState.Idle)
            {
                isGathering = true;
            }                
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
        hasChargedThrow = false;
    }

    public void ChargeThrow()//Gets the time which the player started charging a throw
    {
        if(isBoosting == false && actionState == ActionState.Idle)
        {
            throwHoldDownStartTime = Time.time;
            hasChargedThrow = true;
        }        
    }

    public float CalculateThrowForce(float throwHoldTime)//calculates what the throw force should be based on how long the throw has been charging for
    {     
        holdTimeNormalised = Mathf.Clamp01(throwHoldTime / maxForceHoldDownTime);
        float force = holdTimeNormalised * maxThrowForce;
        force = force + minThrowForce;
        return force;
    }

    public void ThrowBall()//throws the ball infront of the player relative to the direction they are facing and changes its velocity based on how long the throw button was held for
    {
        if(hasChargedThrow)
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
            hasChargedThrow = false;
        }        
    }

    public void AttemptHit()
    {
        if (actionState == ActionState.Idle && Time.time >= attackStateTime + nextAttackTimeIncrement && isBoosting == false && hasChargedThrow == false && isGathering == false)
        {
            actionState = ActionState.AttackWindup;
            attackStateTime  = Time.time;


            //PRE-ATTACK ANIMATION/EFFECT            
            preHitParticles = Instantiate(preHitChargePrefab, transform.position + (transform.right * preHitPointDistance), transform.rotation) as GameObject;
            preHitParticles.transform.parent = transform;

            
        }
    }

    void Attack()//hits in front of the player; any other players in the colliders in the Hitbox gameobject will die
    {
        Collider2D currentPlayerPolygonCollider = gameObject.FindComponentInChildWithTag<PolygonCollider2D>("Hitbox");
        Collider2D currentPlayerCircleCollider = gameObject.FindComponentInChildWithTag<CircleCollider2D>("Hitbox");

        //ATTACK ANIMATION/EFFECT 
        hitParticles = Instantiate(hitPrefab, transform.position + (transform.right * hitPointDistance), transform.rotation) as GameObject;

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
            if (playerCollider.gameObject.GetComponent<Mover>().playerIndex != this.playerIndex) //playerCollider.name != this.gameObject.name)
            {
                /*Vector2 direction = (playerCollider.transform.position - transform.position)*10000; //intended to shoot the player off the edge really fast like in advance wars so they could explode off the edge like in smash bros; couldnt get it to work
                playerCollider.attachedRigidbody.AddForceAtPosition(direction, transform.position);*/
                KillPlayer(playerCollider.gameObject);
            }
        }        

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
            ballCollider.attachedRigidbody.AddForce(direction.normalized * hitStrengthMultiplier);
        }

        //COMMENCE ATTACK FREEZE - PREVENTS PLAYER FROM DOING ANYTHING ELSE UNTIL FREEZETIME IS OVER
        hitFreezeTime = Time.time + hitFreezeTimeIncrement;
        
    }

    public void KillPlayer(GameObject player)
    {
        //DEATH ANIMATIONS add: https://www.youtube.com/watch?v=uR2jcU3x3kU
        deathParticles = Instantiate(deathParticlesPrefab, player.transform.position, player.transform.rotation) as GameObject;
        player.SetActive(false);
        gameController.PrepPlayerRespawn(player);
        hitParticles.transform.parent = null;
        preHitParticles.transform.parent = null;        
    }

    public void StartBoosting()
    {
        if (actionState != ActionState.AttackWindup && actionState != ActionState.AttackBackswing && hasChargedThrow == false)
        {
            isBoosting = true;
        }
    }

    public void StopBoosting()
    {
        isBoosting = false;
    }

    private void MovePlayer()
    {
        //move player in direction     
        moveDirection = new Vector2(moveInputVector.x, moveInputVector.y);
        Vector2 boostDirection = transform.right;
        if (isBoosting == true && boostCapacity > 0)
        {
            Debug.Log("Boosting");
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
        if (actionState == ActionState.AttackWindup && Time.time >= attackStateTime + hitStartupTimeIncrement)
        {
            Attack();
            Debug.Log("Attack-Backswing");
            actionState = ActionState.AttackBackswing;
            attackStateTime = Time.time;
        }

        if (actionState == ActionState.AttackBackswing && Time.time >= attackStateTime + hitFreezeTimeIncrement)
        {
            actionState = ActionState.Idle;
            attackStateTime = Time.time;
        }   
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
        if (actionState != ActionState.AttackBackswing)
        {
            MovePlayer();
            AimPlayer();
        }

        if (!isBoosting)
        {
            if (boostCapacity <= maxBoostCapacity)
            {
                boostCapacity += Time.fixedDeltaTime; //replenish boost power when not boosting
            }                
        }
    }
}
