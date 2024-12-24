using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DMovement : MonoBehaviour
{

    public float moveSpeed = 5f;


    private Rigidbody2D rb;


    private Vector2 movement;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");


        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {

        rb.velocity = movement * moveSpeed;
    }
}