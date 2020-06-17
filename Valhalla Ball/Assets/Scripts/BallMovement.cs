using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    //[SerializeField]
    //private Vector2 initialVelocity;
    public int ballIndex;

    private Rigidbody2D ballRigidBody;
    Vector2 lastVelocity;
    public bool isOwned = false; 


    void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        //ballRigidBody.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = ballRigidBody.velocity;        
    }

    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        ballRigidBody.velocity = direction * Mathf.Max(speed, 0f);
    }
}
