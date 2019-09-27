using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocityX = 2f;
    public float jumpForce = 20f;

    private Rigidbody2D rb2d;
    private Animator animator;
    private bool facingLeft = true;
    private bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.IsGameOver())
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (facingLeft)
                {
                    Flip();
                }
                rb2d.velocity = new Vector2(velocityX, rb2d.velocity.y);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (!facingLeft)
                {
                    Flip();
                }
                rb2d.velocity = new Vector2(-velocityX, rb2d.velocity.y);
            }
            if (grounded && Input.GetKey(KeyCode.W))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    WeaponController.instance.Attack();
            //    animator.SetTrigger("Attack");

            //}
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            rb2d.mass = 0;
        }


        animator.SetFloat("SpeedX", Mathf.Abs(rb2d.velocity.x));
        animator.SetFloat("SpeedY", Mathf.Abs(rb2d.velocity.y));
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }
    }

}
