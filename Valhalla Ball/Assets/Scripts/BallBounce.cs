using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    [SerializeField]
    private Vector2 initialVelocity;

    private Rigidbody2D ballRigidBody;

    Vector2 lastVelocity;

    void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        ballRigidBody.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = ballRigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        ballRigidBody.velocity = direction * Mathf.Max(speed, 0f);
    }
}
