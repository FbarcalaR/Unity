﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private float groundHorizontalLenght;

    // Start is called before the first frame update
    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLenght = groundCollider.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -groundHorizontalLenght)
        {
            RepositionBackGround();
        }
    }

    private void RepositionBackGround()
    {
        Vector2 groundOffset = new Vector2(groundHorizontalLenght * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
