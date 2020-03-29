using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float force = 3f;
    public float maxVelocity;
    public GameObject whereScores;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
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
            else if (Input.GetKey(KeyCode.A))
            {
                rb2d.AddForce(new Vector2(-force, 0f));
                if (rb2d.velocity.x < -maxVelocity)
                {
                    rb2d.velocity = new Vector2(-maxVelocity, rb2d.velocity.y);
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
            else if (Input.GetKey(KeyCode.D))
            {
                rb2d.AddForce(new Vector2(force, 0f));
                if (rb2d.velocity.x > maxVelocity)
                {
                    rb2d.velocity = new Vector2(maxVelocity, rb2d.velocity.y);
                }
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == whereScores.GetComponent<Collider2D>())
        {
            GameController.instance.GameOver();
            GameController.instance.TeamScored(whereScores);
        }
    }

}
