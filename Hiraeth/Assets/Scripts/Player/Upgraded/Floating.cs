using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("References")]
    private Movement movement;
    public Gravity gravityScript;

    [Header("Floating Settings")]
    public float upwardGravity = 45;
    public float floatingSpeed = 1f;
    public float heightAboveGround = 3f;
    [SerializeField] private float speedDuration = 0.5f;
    public bool isFloating = false;

    [Header("Keybinds")]
    public KeyCode firstKey = KeyCode.Mouse0;
    public KeyCode secondKey = KeyCode.Mouse1;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        bool closeToGround = Physics.Raycast(transform.position, Vector3.down, heightAboveGround, movement.whatIsGround);
        bool isFalling = !movement.grounded && movement.rb.linearVelocity.y < -0.1f;

        if (Input.GetKey(firstKey) && Input.GetKey(secondKey) && !closeToGround && isFalling)
        {
            isFloating = true;
        }
        else
        {
            isFloating = false;
        }
    }

    void FixedUpdate()
    {
        if (movement.state == Movement.MovementState.Air && isFloating)
        {
            gravityScript.enabled = false;
            Vector3 velocity = movement.rb.linearVelocity;
            velocity.y += upwardGravity * Time.fixedDeltaTime;

            movement.moveSpeed = Mathf.MoveTowards(movement.walkSpeed, floatingSpeed, speedDuration);
        }
        else
        {
            gravityScript.enabled = true;
        }
    }
}
