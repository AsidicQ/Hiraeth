using System.Collections;
using UnityEngine;

public class GroundDash : MonoBehaviour
{
    [Header("References")]
    private Movement movement;
    private CapsuleCollider playerCollider;
    public CameraBobbing cameraBobbing;

    [Header("Dash Settings")]
    public float dashDistance = 15f;
    public float dashDuration = 0.2f;
    public AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve cameraCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public KeyCode dashKey = KeyCode.LeftAlt;
    public LayerMask collisionMask = ~0;

    [Header("Collision Settings")]
    public float skin = 0.01f;

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
        movement.canMove = false;
        cameraBobbing.enabled = false;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = movement.orientation.forward * verticalInput
            + movement.orientation.right * horizontalInput;
        if (moveDirection.sqrMagnitude < 0.01f)
        {
            moveDirection = -movement.orientation.forward;
        }

        LayerMask dashMask = collisionMask & ~movement.whatIsGround;

        moveDirection.y = 0f;
        moveDirection.Normalize();

        float elapsedTime = 0f;
        float previousTime = 0f;

        while (elapsedTime < dashDuration)
        {
            yield return new WaitForFixedUpdate();

            elapsedTime += Time.fixedDeltaTime;

            float time = Mathf.Clamp01(elapsedTime / dashDuration);

            float curveNow = dashCurve.Evaluate(time);
            float curvePrev = dashCurve.Evaluate(previousTime);
            float stepPortion = Mathf.Max(0f, curveNow - curvePrev);

            float cameraCurveNow = cameraCurve.Evaluate(time);
            float cameraCurvePrev = cameraCurve.Evaluate(previousTime);
            float cameraPortion = Mathf.Max(0f, cameraCurveNow - cameraCurvePrev);

            float stepDistance = dashDistance * stepPortion;
            float cameraAmount = cameraPortion * dashDistance;

            if (stepDistance <= 0f)
            {
                previousTime = time;
                continue;
            }

            if (cameraAmount <= 0f)
            {
                continue;
            }

            // Apply camera bobbing effect

            float maxMove = stepDistance;
            float currentY = movement.rb.linearVelocity.y;
            if (TryGetBlockedDistance(moveDirection, maxMove + skin, dashMask, out float hitDist,
                out Vector3 hitNormal))
            {
                float allowed = hitDist - skin;
                if (allowed <= 0f)
                {
                    Debug.Log("Dash blocked immediately.");
                    break;
                }

                float stepSpeed = allowed / Time.fixedDeltaTime;

                float yAxis = movement.rb.linearVelocity.y;
                Vector3 v = moveDirection * stepSpeed + Vector3.up * yAxis;

                v = Vector3.ProjectOnPlane(v, hitNormal);

                if (v.y > 0f) v.y = 0f;

                movement.rb.linearVelocity = v;

                previousTime = time;
                break;
            }
            float speed = maxMove / Time.fixedDeltaTime;
            float y = movement.rb.linearVelocity.y;
            movement.rb.linearVelocity = moveDirection * speed + Vector3.up * y;

            previousTime = time;
        }

        movement.canMove = true;
        cameraBobbing.enabled = true;
        isDashing = false;
    }

    bool TryGetBlockedDistance(Vector3 moveDirection, float maxDistance, LayerMask mask, out float hitDistance, out Vector3 hitNormal)
    {
        GetCapsuleWorldPoints(movement.rb.position, out Vector3 point1, out Vector3 point2, out float radius);

        radius = Mathf.Max(0f, radius - skin);

        if (Physics.CapsuleCast(point1, point2, radius, moveDirection, out RaycastHit hit, 
            maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            hitDistance = hit.distance;
            hitNormal = hit.normal;
            return true;
        }

        hitDistance = 0f;
        hitNormal = Vector3.up;
        return false;
    }

    void GetCapsuleWorldPoints(Vector3 rbPosition, out Vector3 point1, out Vector3 point2, out float radius)
    {
        Transform ct = playerCollider.transform;
        Vector3 center = rbPosition + (ct.rotation * playerCollider.center);

        float scaleXZ = Mathf.Max(ct.lossyScale.x, ct.lossyScale.z);
        radius = playerCollider.radius * scaleXZ;

        float height = Mathf.Max(playerCollider.height * ct.lossyScale.y, radius * 2f);
        float halfHeight = height * 0.5f - radius;

        Vector3 up = ct.up;
        point1 = center + up * halfHeight;
        point2 = center - up * halfHeight;
    }
}
