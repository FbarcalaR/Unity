using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float upForce = 200f;
    public AudioClip flapSound;
    public AudioClip hitSound;

    private AudioSource audioSource;
    private bool isDead=false;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = flapSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                audioSource.Play();
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0f, upForce));
                anim.SetTrigger("Flap");
            }

        }
    }

    private void OnCollisionEnter2D()
    {
        if (!isDead)
        {
            rb2d.velocity = Vector2.zero;
            isDead = true;
            anim.SetTrigger("Die");

            GameControl.instance.BirdDied();

            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }
}
