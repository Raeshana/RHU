using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Animator _anim;

    // movement
    [SerializeField] private float _moveSpeed = 5f;
    private float _moveDirection; // -1: left, 1: right, 0: not moving
    private bool _isFacingRight = true;


    // jumps
    private bool _isGrounded;
    [SerializeField] private Transform _groundCheck; // isGrounded capsule overlay
    [SerializeField] private LayerMask _groundLayer; // Ground Layer
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private float _maxFallSpeed = 5f; // clamp fall
    [SerializeField] private float _coyoteTime = 0.2f; // max time for player to perform a delayed jump
    private float _coyoteTimeCounter = 0f; // coyote time countdown

    [SerializeField] private float _jumpBufferTime = 0.2f; // max time before landing that a player can jump again
    private float _jumpBufferCounter = 0f; // jump buffer time countdown

    private bool _doubleJump; // true: can double jump, false: cannot double jump

    // Thrust
    private bool _canThrust = true;
    //private bool _isThrusting; 
    public float _thrustSpeed = 10f;
    public float _thrustTime = 0.2f;
    public float _thrustCooldown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = Input.GetAxisRaw("Horizontal");

        Jump();
        ThrustFn();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Flip() {           
        if (_moveDirection < 0f) {
            _isFacingRight = false;
            _sr.flipX = true; // flips sprite to the left
            //cameraFollow.CallTurn();
        }
        else {
            _isFacingRight = true;
            _sr.flipX = false; // flips sprite to the right
           // cameraFollow.CallTurn();
        }
    }

    private void Move()
    {
        if (_moveDirection != 0f)
        {
            Flip();
            _rb.velocity = new Vector2(_moveDirection * _moveSpeed, _rb.velocity.y);
        }
    }

    private void Jump() {      
        _isGrounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(0.4f, 0.08f), CapsuleDirection2D.Horizontal, 0f, _groundLayer);

        if (_isGrounded && !Input.GetButton("Jump")) {
            _doubleJump = false;
        }

        if (_isGrounded) {
            _coyoteTimeCounter = _coyoteTime;  // resets coyote timer
        }
        else {
            _coyoteTimeCounter -= Time.deltaTime; // counts down coyote timer 
        }

        if (Input.GetButtonDown("Jump")) {
            _jumpBufferCounter = _jumpBufferTime; // resets buffer timer 
        }
        else {
            _jumpBufferCounter -= Time.deltaTime; // counts down buffer timer
        }

        if (_jumpBufferCounter > 0f && (_coyoteTimeCounter > 0f || _doubleJump)) {
            _anim.SetBool("isJumping", true);
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpSpeed);
            _doubleJump = !_doubleJump;
            _jumpBufferCounter = 0f; // resets buffer timer
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f) {
            _coyoteTimeCounter = 0f; // resets coyote timer
        }

        if (_rb.velocity.y < 0f) {
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isFalling", true);
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_maxFallSpeed, float.MaxValue)); // clamp fall speed
        }
    }

    private void ThrustFn() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrust) {
            StartCoroutine(Thrust());
        }
    }

    private IEnumerator Thrust() {
        _canThrust = false;
        //_isThrusting = true;

        // diable gravity for thrust
        float _gravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        // thrust
        if (_isFacingRight) {
            _rb.velocity = new Vector2(_thrustSpeed, 0f); 
        }
        else {
            _rb.velocity = new Vector2(-_thrustSpeed, 0f); 
        }
        yield return new WaitForSeconds(_thrustTime);

        // reset gravity
        _rb.gravityScale = _gravity;
        //_isThrusting = false;

        // dash cooldown
        yield return new WaitForSeconds(_thrustCooldown);
        _canThrust = true;
    }

    public void EndFall()
    {
        if (_isGrounded)
        {
            _anim.SetBool("isFalling", false);
        }
    }
}
