using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnCSharp {

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlatformerMovement : MonoBehaviour
    {
        [SerializeField] private bool holdJump = true;
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpPower = 16f;
        [SerializeField] private LayerMask groundLayer;

        private bool facingRight = true;
        private float horizontal;

        private Rigidbody2D rb;
        private Animator animator;
        private Transform groundCheck;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            groundCheck = transform.GetChild(0).transform;
        }

        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            animator.SetFloat("Speed", Math.Abs(horizontal));

            if(Input.GetButtonDown("Jump") && IsGounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            
            if (holdJump && Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {   
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            HandleFlip();
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        private void HandleFlip()
        {
            if((facingRight && horizontal < 0f) || (!facingRight && horizontal > 0f))
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        private bool IsGounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

    }

}