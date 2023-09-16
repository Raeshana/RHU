using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    // movement
    [SerializeField] private float _moveSpeed;
    private float _moveDirection; // -1: left, 1: right, 0: not moving

    // jump
    private bool _isGrounded;
    [SerializeField] Transform _groundCheck; // isGrounded capsule overlay
    [SerializeField] LayerMask _groundLayer; // Ground Layer
    [SerializeField] float _jumpSpeed;
    [SerializeField] float _maxFallSpeed; // clamp fall
    [SerializeField] float _coyoteTime; // max time for player to perform a delayed jump
    private float _coyoteTimeCounter = 0f; // coyote time countdown

    [SerializeField] float _jumpBufferTime; // max time before landing that a player can jump again
    private float _jumpBufferCounter = 0f; // jump buffer time countdown

    private bool _doubleJump; // true: can double jump, false: cannot double jump

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_moveDirection != 0f)
        {
            _rb.velocity = new Vector2(_moveDirection * _moveSpeed, rb.velocity.y);
        }
    }
}
