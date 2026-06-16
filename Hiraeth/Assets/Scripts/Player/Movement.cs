using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public bool isSprinting;
    public bool canSprint;
    public float accelerationSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    [SerializeField] private bool readyToJump;
    [SerializeField] private bool doubleJumped;

    [Header("Ground Check")]
    float groundDrag = 5f;
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Management")]
    public float maxSlopeAngle;
    public float slidingThreshold;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode walkKey = KeyCode.LeftControl;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public static bool canMove = true;
    public bool sprintFromDash = false;

    public Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        Running,
        Sprinting,
        Walking,
        Air
    }

    private void StateHandler()
    {
        bool sliding = Physics.Raycast(transform.position, Vector3.down, slidingThreshold, whatIsGround);
        bool tooSteep = OnSlope() && Vector3.Angle(Vector3.up, slopeHit.normal) > maxSlopeAngle;

        if (grounded && (Input.GetKey(sprintKey) || sprintFromDash) && canSprint && (rb.linearVelocity.magnitude >= 0.1f))
        {
            state = MovementState.Sprinting;

            if (state == MovementState.Sprinting)
            {
                isSprinting = true;
                moveSpeed = Mathf.MoveTowards(moveSpeed, sprintSpeed, accelerationSpeed * Time.deltaTime);
            }
        }

        else if (Input.GetKey(walkKey) || tooSteep)
        {
            state = MovementState.Walking;
            moveSpeed = walkSpeed;
            isSprinting = false;
            canSprint = false;
        }

        else if (grounded)
        {
            state = MovementState.Running;
            isSprinting = false;
            canSprint = true;
            moveSpeed = Mathf.MoveTowards(moveSpeed, runSpeed, accelerationSpeed * Time.deltaTime);
        }

        else if (sliding && Input.GetKey(sprintKey) && canSprint)
        {
            state = MovementState.Sprinting;
            isSprinting = true;
            moveSpeed = Mathf.MoveTowards(moveSpeed, sprintSpeed, accelerationSpeed * Time.deltaTime);
        }

        else
        {
            state = MovementState.Air;
            isSprinting = false;
        }
    }

    private void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        canSprint = true;
        doubleJumped = false;

        moveSpeed = runSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        rb.linearDamping = grounded ? groundDrag : 0f;

        if (canMove)
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }

        if (sprintFromDash && !Input.GetKey(KeyCode.W))
        {
            sprintFromDash = false;
        }

        if (grounded && doubleJumped)
        {
            doubleJumped = false;
            readyToJump = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (canMove)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            return;
        }

        if (Input.GetKeyDown(jumpKey) && readyToJump)
        {
            if (grounded)
            {
                Jump();
                doubleJumped = false;
                Invoke(nameof(ResetJump), jumpCoolDown);
            }
            else if (!doubleJumped && !grounded)
            {
                doubleJumped = true;
                readyToJump = false;
                Jump();
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 15f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 30f, ForceMode.Force);
            }

            rb.useGravity = false; 
            return;
        }

        rb.useGravity = true;

        if (grounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float castDistance = playerHeight * 0.5f + 0.6f;

        if (Physics.SphereCast(origin, slidingThreshold, Vector3.down, out slopeHit, castDistance, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > 0 && angle < maxSlopeAngle;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
