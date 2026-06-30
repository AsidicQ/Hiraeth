using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityFallRate;
    public Movement movement;

    public static bool isGravityEnabled = true;

    void FixedUpdate()
    {
        if (isGravityEnabled && movement.state == Movement.MovementState.Air)
        {
            Vector3 velocity = movement.rb.linearVelocity;
            velocity.y += -gravityFallRate * Time.fixedDeltaTime;

            movement.rb.linearVelocity = velocity;
        }
    }
}
