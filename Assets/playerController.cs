using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    // movement
    [SerializeField] private float _moveSpeed;
    private float _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        _rbrb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (_moveDirection != 0f)
        {
            _rb.velocity = new Vector2(_moveDirection * _moveSpeed, rb.velocity.y);
        }
    }
}
