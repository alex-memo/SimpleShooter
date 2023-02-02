using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// @memo 2023
/// Script for the player movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    private float speed;
    private readonly float walkSpeed = 3f;
    private readonly float runSpeed = 5f;

    private readonly float turnSmoothTime = .2f;
    private float turnSmoothVelocity;

    private readonly float jumpHeight = 1.25f;
    private readonly float gravity = -9.81f;
    private bool isGrounded;
    private readonly float groundCheckDistance = .2f;

    private Vector3 velocity;

    private Vector3 direction;

    private Transform cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private bool isRunning;
    private Animator anim;
    /// <summary>
    /// Sets variables
    /// </summary>
    private void Awake()
    {
        anim= GetComponent<Animator>();
        cam = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }
    /// <summary>
    /// Calls the handlers
    /// </summary>
    private void Update()
    {
        movePlayer();
        move();
    }
    /// <summary>
    /// Handles gravity and movement
    /// </summary>
    private void FixedUpdate()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    /// <summary>
    /// @memo 2023
    /// Handles the player movement
    /// </summary>
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
    /// <summary>
    /// @memo 2023
    /// Checks for states of movement & grounded
    /// </summary>
    private void movePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded && direction == Vector3.zero)
        {
            idle();
        }
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to set the movement value
    /// </summary>
    /// <param name="_value"></param>
    private void OnMovement(InputValue _value)
    {
        Vector2 _v2 = _value.Get<Vector2>();
        direction = new Vector3(_v2.x,0,_v2.y);
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to run
    /// </summary>
    /// <param name="_value"></param>
    private void OnRun(InputValue _value)
    {
        isRunning = _value.Get<float>() > 0;
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to jump
    /// </summary>
    private void OnJump()
    {
        if (isGrounded)
        {
            triggerAnim("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);           
        }
    }
    /// <summary>
    /// @memo 2023
    /// Sets the running attributes to the player
    /// </summary>
    private void run()
    {
        speed = runSpeed;
        if (isGrounded)
        {
            animate(1f, .1f);
        }
    }
    /// <summary>
    /// @memo 2023
    /// Sets the walking attributes to the player
    /// </summary>
    private void walk()
    {
        speed = walkSpeed;
        if (isGrounded)
        {
            animate(.5f, .1f);
        }
    }
    /// <summary>
    /// @memo 2023
    /// Sets the idle attributes to the player
    /// </summary>
    private void idle()
    {
        if (isGrounded)
        {
            animate(0, 0);
        }
    }
    /// <summary>
    /// @memo 2023
    /// Animates the player on call via the recieved params
    /// </summary>
    /// <param name="_value">The Speed value to set</param>
    /// <param name="_dampTime">The damp time for the animation</param>
    private void animate(float _value, float _dampTime=0)
    {
        anim.SetFloat("Speed", _value, _dampTime, Time.deltaTime);
    }
    /// <summary>
    /// @memo 2023
    /// Triggers the animation recieved
    /// </summary>
    /// <param name="_trigger"></param>
    private void triggerAnim(string _trigger)
    {
        anim.SetTrigger(_trigger);
    }
}
