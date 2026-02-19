using UnityEngine;

public class Floating : MonoBehaviour
{
    private Movement movement;

    [Header("Floating Settings")]
    public float upwardGravity = 45;
    public float floatingSpeed = 1f;
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
        if (Input.GetKey(firstKey) && Input.GetKey(secondKey))
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
            Vector3 velocity = movement.rb.linearVelocity;
            velocity.y += upwardGravity * Time.fixedDeltaTime;

            movement.rb.linearVelocity = velocity;
            movement.moveSpeed = Mathf.MoveTowards(movement.walkSpeed, floatingSpeed, speedDuration);
        }
    }
}
