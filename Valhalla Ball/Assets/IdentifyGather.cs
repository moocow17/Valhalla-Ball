using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyGather : MonoBehaviour
{
    private new GameObject gameObject;
    private Mover gameObjectsMover;

    private void Awake()
    {
        gameObject = GetComponent<GameObject>();
        gameObject = this.transform.parent.gameObject;
        gameObjectsMover = gameObject.GetComponent<Mover>();
    }

    private void Gather(Collider2D collision)
    {
        gameObjectsMover.hasBall = true;
        gameObjectsMover.isGathering = false;
        collision.attachedRigidbody.isKinematic = true;
        collision.enabled = false;
        collision.transform.position = gameObject.transform.position;
        collision.transform.parent = gameObject.transform;
    }

    private void GatherBall(Collider2D collision)
    {
        if (gameObjectsMover.isGathering)
        {
            if (collision.CompareTag("Player"))
            {
                Mover playerMover = (Mover)collision.gameObject.GetComponent(typeof(Mover));
                //Mover playerMover = Helper.FindComponentInChildWithTag<Mover>(collision.gameObject, "Player");
                GameObject otherPlayerGameObject = playerMover.gameObject;
                if(playerMover.hasBall)
                {
                    //move ball to new player
                    Collider2D ballCollider = Helper.FindComponentInChildWithTag<Collider2D>(collision.gameObject, "Ball");
                    Gather(ballCollider.GetComponent<Collider2D>());
                    //set other player's hasBall property to false
                    playerMover.hasBall = false;
                }
                return;
            }
            if (collision.CompareTag("Ball"))
            {
                Gather(collision);
            }
        }


        /* function OnTriggerEnter(other : Collider)
{
         if (other.gameObject.CompareTag("Attack"))
         {
             damage = other.GetComponent(AttackTemplate).power - defense;

             /*ACTIVATES COLLIDER WHICH WHEN IT COMES INTO CONTACT WITH BALL OR CARRIER WILL GIVE BALL TO THIS PLAYER
              * check if collider is hitting ball, if so and if player's isGathering is true:
              * {
              *  move ball instance to centre of player and attach it so it follows
              *  set a variable in the player to indicate they own the ball (is this a race condition if two players grab ball?)
              *  change the same variable in all other players to indicate they no longer own the ball?
              * }
              * else:
              * {
              *  if isGathering is true: loop through other players and if connecting with another player who has the ball do same as above
              * } */
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        GatherBall(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GatherBall(collision);
    }
}
