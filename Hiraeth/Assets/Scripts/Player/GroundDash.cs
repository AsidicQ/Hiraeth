using System.Collections;
using UnityEngine;

public class GroundDash : MonoBehaviour
{
    private Movement movement;
    private CapsuleCollider playerCollider;

    [Header("Dash Settings")]
    public float dashDistance = 15f;
    public float dashDuration = 0.2f;
    public AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public KeyCode dashKey = KeyCode.LeftAlt;
    public LayerMask collisionMask = ~0;

    private bool isDashing;

    void Start()
    {
        movement = GetComponent<Movement>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();
    }

    void Update()
    {

        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        if (!movement.grounded)
            yield break;

        isDashing = true;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = movement.orientation.forward * verticalInput
            + movement.orientation.right * horizontalInput;
        if (moveDirection.sqrMagnitude < 0.01f)
        {
            moveDirection = -movement.orientation.forward;
        }

        moveDirection.y = 0f;
        moveDirection.Normalize();

        Vector3 velocity = movement.rb.linearVelocity;
        movement.rb.linearVelocity = new Vector3(0f, velocity.y, 0f);

        float elapsedTime = 0f;
        float previousTime = 0f;

        while (elapsedTime < dashDuration)
        {
            yield return new WaitForFixedUpdate();

            elapsedTime += Time.fixedDeltaTime;

            float time = Mathf.Clamp01(elapsedTime / dashDuration);

            float curveNow = dashCurve.Evaluate(time);
            float curvePrev = dashCurve.Evaluate(previousTime);
            float stepPortion = curveNow - curvePrev;

            Vector3 dash = moveDirection * (dashDistance * stepPortion);
            float dashDist = dash.magnitude;

            if (dashDist > 0f)
            {
                Vector3 position = movement.rb.position;

                GetCapsuleWorldPoints(position, out Vector3 point1, out Vector3 point2, out float radius);

                if (Physics.CapsuleCast(point1, point2, radius, moveDirection, out RaycastHit hit, dashDist,
                    collisionMask, QueryTriggerInteraction.Ignore))
                {
                    float inset = 0.01f;
                    movement.rb.MovePosition(position + moveDirection * Mathf.Max(0f, hit.distance - inset));
                    break;
                }

                movement.rb.MovePosition(position + dash);
            }

            previousTime = time;
        }
        isDashing = false;
    }

    void GetCapsuleWorldPoints(Vector3 rbPosition, out Vector3 point1, out Vector3 point2, out float radius)
    {
        Transform ct = playerCollider.transform;
        Vector3 center = ct.TransformPoint(playerCollider.center);

        float scaleXZ = Mathf.Max(ct.lossyScale.x, ct.lossyScale.z);
        radius = playerCollider.radius * scaleXZ;

        float height = Mathf.Max(playerCollider.height * ct.lossyScale.y, radius * 2f);
        float halfHeight = height * 0.5f - radius;

        Vector3 up = ct.up;
        point1 = center + up * halfHeight;
        point2 = center - up * halfHeight;
    }
}
