using UnityEngine;
[RequireComponent(typeof(Controller))]
public abstract class MovementScript : MonoBehaviour
{
    protected CharacterController characterController;

    protected float speed;
    protected readonly float walkSpeed = 3f;
    protected readonly float runSpeed = 5f;

    protected readonly float turnSmoothTime = .2f;
    protected float turnSmoothVelocity;

    protected readonly float jumpHeight = 1.25f;
    protected readonly float gravity = -9.81f;
    protected bool isGrounded=> Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    protected readonly float groundCheckDistance = .2f;

    protected Vector3 velocity;

    protected Vector3 direction;

    protected Transform cam;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundMask;

    protected bool isRunning;
    protected Animator anim;
    /// <summary>
    /// Sets variables
    /// </summary>
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }
    /// <summary>
    /// Handles gravity and movement
    /// </summary>
    protected void FixedUpdate()
    {
        if(characterController!= null)
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }      
    }
    /// <summary>
    /// Calls the handlers
    /// </summary>
    protected virtual void Update()
    {
        movePlayer();
        move();
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
    /// Sets the running attributes to the player
    /// </summary>
    protected void run()
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
    protected void walk()
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
    protected void idle()
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
    protected void animate(float _value, float _dampTime = 0)
    {
        anim.SetFloat("Speed", _value, _dampTime, Time.deltaTime);
    }
    /// <summary>
    /// @memo 2023
    /// Triggers the animation recieved
    /// </summary>
    /// <param name="_trigger"></param>
    protected void triggerAnim(string _trigger)
    {
        anim.SetTrigger(_trigger);
    }
}
