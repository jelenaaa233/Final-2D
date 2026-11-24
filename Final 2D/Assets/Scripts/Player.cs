using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _kickLeft;
    [SerializeField] private GameObject _kickRight;
    [SerializeField] private GameController _gameController;
    // Add member variables to store references to this GameObject's 
    //      Rigidbody, Animator, and SpriteRenderer.
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed;
    [SerializeField] private float _jump;

    private bool _facingLeft;
    private bool _isGrounded;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ------------------------------------------------------------------------
    // Add a built-in UNITY METHOD that runs EVERY FRAME.
        // Detect input to move player left/right, jump, and kick
        // Change character facing with SpriteRenderer.flipX value
        // Set animation parameters 
    private void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        _rb.linearVelocity = new Vector2(move * _speed, _rb.linearVelocity.y);

        if (move < 0)
        {
            _facingLeft = true;
            _spriteRenderer.flipX = true;
        }
        
        else if (move > 0)
        {
            _facingLeft = false;
            _spriteRenderer.flipX = false;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jump);

            _isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Kick();
        }

        _animator.SetBool("isWalking", move != 0);
        _animator.SetBool("isJumping", !_isGrounded);
    }

    // ------------------------------------------------------------------------
    private void Kick()
    {
        // Set animation parameter.
        _animator.SetTrigger("Kick");

        // Either activate left or right kick based on character facing
        if (_facingLeft)
        {
            _kickLeft.SetActive(true);
        }

        else
        {
            _kickRight.SetActive(true);
        }
    }

    // ------------------------------------------------------------------------
    // Use a built-in Unity method to detect COLLISION.
    // Use this to avoid allowing the Player to double jump- they should
    //      collide with an object with the Ground tag before being able to
    //      jump again.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    _isGrounded = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
