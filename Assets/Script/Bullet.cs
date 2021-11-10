using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    PlayerMovement playerMovement;
    float speed;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        speed = playerMovement.transform.localScale.x;
    }

    void Update()
    {
        rigidbody2D.velocity = new Vector2(20f * speed, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
