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
