using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeed : MonoBehaviour
{
    // Speed that ball picks up when bouncing off objects
    public float bounceSpeed = 5;
    public float bounceCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // gets the direction to go 
        Vector2 normal = collision.GetContact(0).normal;
        // applies the direction with new bounce spped on top
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(normal * bounceSpeed);
        if (bounceCount < 0.4f) bounceCount += 0.01f;
    }
}
