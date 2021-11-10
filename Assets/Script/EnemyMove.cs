using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speedEnemy = 3f;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D boxCollider2D;
    new Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = new Vector2(speedEnemy, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speedEnemy = -speedEnemy;
        FlipFacing();
    }

    public void FlipFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, 1);
    }
}
