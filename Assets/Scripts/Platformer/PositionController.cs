using System;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Animator _animator;
    GroundDetector _groundDetector;

    [SerializeField] float runningSpeed = 10;

    [SerializeField] float hAcceleration = 30;

    [SerializeField] float jumpImpulse = 30;

    int dir = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float vx = 0;

        float dx;// = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftArrow)) dx = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) dx = 1;
        else dx = 0;

        //_rigidbody.linearVelocityX = dx * runningSpeed;

        float dhSpeed = hAcceleration * Time.deltaTime;

        if (dx == 0)
        {
            if (_rigidbody.linearVelocityX > 0)
            {
                vx = _rigidbody.linearVelocityX - dhSpeed;
                if (vx < 0) vx = 0;
            }
            else if (_rigidbody.linearVelocityX < 0)
            {
                vx = _rigidbody.linearVelocityX + dhSpeed;
                if (vx > 0) vx = 0;
            }
        }
        else
        {
            vx = _rigidbody.linearVelocityX + dx*dhSpeed;
            vx = Mathf.Clamp(vx, -runningSpeed, runningSpeed);
        }
        
        _rigidbody.linearVelocityX = vx;


        if (Input.GetKey(KeyCode.UpArrow) && _groundDetector.IsGrounded && _rigidbody.linearVelocityY<=0)
        {
            _rigidbody.linearVelocityY = 0;
            _rigidbody.AddForceY(jumpImpulse, ForceMode2D.Impulse);
        }

        _animator.SetFloat("Vx", MathF.Abs(vx));

        if (dx > 0) dir = 1;
        if (dx < 0) dir = -1;

        transform.localScale = new(dir, 1, 1);


    }
}
