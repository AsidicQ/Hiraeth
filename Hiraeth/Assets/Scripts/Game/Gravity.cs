using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityFallRate;
    public Movement movement;

    void FixedUpdate()
    {
        if (movement.state == Movement.MovementState.Air)
        {
            Vector3 velocity = movement.rb.linearVelocity;
            velocity.y += -gravityFallRate * Time.fixedDeltaTime;

            movement.rb.linearVelocity = velocity;
        }
    }
}
