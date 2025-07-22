using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeed : MonoBehaviour
{
    public float bounceSpeed = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(normal * bounceSpeed);
    }
}
