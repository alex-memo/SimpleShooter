using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController characterController;
    private float speed;
    private readonly float walkSpeed = 3f;
    private readonly float runSpeed = 5f;

    private float turnSmoothTime = .2f;
    private float turnSmoothVelocity;

    private float jumpHeight = 1.25f;
    private float gravity = -9.81f;
    private bool isGrounded;
    private float groundCheckDistance = .2f;

    private Vector3 velocity;

    private Vector3 direction;

    public Transform cam { get; set; }
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private bool isRunning;

    private void Awake()
    {
        cam = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        movePlayer();
        move();
    }
    private void FixedUpdate()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    private void move()
    {
        if (direction.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //run
            if (movedir != Vector3.zero && isRunning)
            {
                run();
            }
            else if (movedir != Vector3.zero && !isRunning)
            {
                walk();
            }
            characterController.Move(speed * Time.deltaTime * movedir.normalized);
        }


    }
    private void movePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded)
        {
            if (direction == Vector3.zero)
            {
                //idle();
            }
        }
        else
        {

        }

    }
    private void OnMovement(InputValue _value)
    {
        Vector2 _v2 = _value.Get<Vector2>();
        direction = new Vector3(_v2.x,0,_v2.y);
        Debug.Log("movement "+_value.Get<Vector2>());
    }
    private void OnRun(InputValue _value)
    {
        isRunning = _value.Get<float>() > 0;
        //print(_ctx.performed);
        //direction = new Vector3(_v2.x, 0, _v2.y);
        //Debug.Log("movement " + _value.Get<Vector2>());
    }
    private void OnJump()
    {
        //direction = new Vector3(_v2.x, 0, _v2.y);
        Debug.Log("jump");
    }
    void run()
    {
        speed = runSpeed;
        if (isGrounded)
        {
            //animate(1f, .1f);
        }
    }
    void walk()
    {
        speed = walkSpeed;
        if (isGrounded)
        {
            //animate(.5f, .1f);
        }
    }
    public void idle()
    {
        if (isGrounded)
        {
            //animate(0, 0);
        }
    }
}
