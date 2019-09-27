using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBar : MonoBehaviour
{
    public float force = 3f;
    public float maxVelocity;
    public float throwbackForceMax = 60f;
    public float throwbackForceMin = 20f;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.IsGameOver())
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0f, force));
                if (rb2d.velocity.y > maxVelocity)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, maxVelocity);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb2d.AddForce(new Vector2(0f, -force));
                if (rb2d.velocity.y < -maxVelocity)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, -maxVelocity);
                }
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            float x = 0f;
            if (tag == "Bar")
            {
                x = Random.Range(throwbackForceMin, throwbackForceMax);
            }
            else if (tag == "EnemyBar")
            {
                x = Random.Range(-throwbackForceMax, -throwbackForceMin);
            }
            float y = Random.Range(-throwbackForceMax, throwbackForceMax);

            Vector2 throwbackVector = new Vector2(x, y);
            Rigidbody2D collisionRb2d = collision.gameObject.GetComponent<Rigidbody2D>();

            collisionRb2d.AddForce(throwbackVector);
        }
    }

}
