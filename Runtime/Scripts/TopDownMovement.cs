using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnCSharp {

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TopDownMovement : MonoBehaviour
    {
        [Header("Player Preferences")]
        [SerializeField]
        private bool faceMouse = true;
        [SerializeField]
        private float faceMouseRange = 0.3f;

        [Header("Movement")]
        public float moveSpeed = 5f;
        public bool canMove = true;

        private Rigidbody2D rb;
        private Animator anim;

        private Vector2 moveDirection = Vector2.zero;
        private Vector2 faceDirection = Vector2.zero;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>(); 
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            HandleMovement();
        }

        void HandleMovement()
        {
            moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            if (faceMouse)
            {
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) >= (faceMouseRange + 0.2f))
                {
                    faceDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                    anim.SetFloat("Horizontal", faceDirection.x);
                    anim.SetFloat("Vertical", faceDirection.y);
                }
                else if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= faceMouseRange)
                {
                    if (moveDirection != Vector2.zero)
                    {
                        faceDirection = moveDirection;
                        anim.SetFloat("Horizontal", faceDirection.x);
                        anim.SetFloat("Vertical", faceDirection.y);
                    }
                }
            }
            else
            {
                if (moveDirection != Vector2.zero)
                {
                    faceDirection = moveDirection;
                    anim.SetFloat("Horizontal", faceDirection.x);
                    anim.SetFloat("Vertical", faceDirection.y);
                }
            }
            anim.SetFloat("Speed", moveDirection.sqrMagnitude);
        }

        void FixedUpdate()
        {
            MovePlayer(moveDirection, moveSpeed);
        }

        void MovePlayer(Vector2 direction, float speed)
        {
            if (!canMove) 
            {
                rb.MovePosition(rb.position);
                return;
            }
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

}
