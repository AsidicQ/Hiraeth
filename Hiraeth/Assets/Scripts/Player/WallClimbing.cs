using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody))]
public class WallClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    public float climbSpeed = 1.2f;
    public float climbHeight = 8f;
    public float climbActivationThreshold = 1f;
    public float speedToClimbNeeded = 2f;
    public static bool isClimbing = false;

    [Header("Ejection Settings")]
    public float ejectionForce = 5f;
    public float ejectJumpForce = 1.5f;
    private Vector3 wallNormal;

    [Header("References")]
    private Movement movementScript;
    private Rigidbody playerBody;
    private float climbingStartHeight;

    [Header("Keybinds")]
    public KeyCode ejectKey = KeyCode.E;

    void Start()
    {
        movementScript = GetComponent<Movement>();
        playerBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        DetectingClimb();

        if (isClimbing && Input.GetKeyDown(ejectKey))
        {
            EjectWall();
        }
    }

    void DetectingClimb()
    {
        if (!Input.GetKey(KeyCode.W) || !Input.GetKey(movementScript.jumpKey))
        {
            isClimbing = false;
            Gravity.isGravityEnabled = true;
            return;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(movementScript.jumpKey))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, climbActivationThreshold))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                {
                    if (playerBody.linearVelocity.magnitude >= speedToClimbNeeded)
                    {
                        ClimbForce();
                        wallNormal = hit.normal;
                    }
                }
                else
                {
                    isClimbing = false;
                    Gravity.isGravityEnabled = true;
                }
            }
        }
    }

    void ClimbForce()
    {
        if (!isClimbing)
        {
            climbingStartHeight = transform.position.y;
        }

        if (transform.position.y >= climbingStartHeight + climbHeight)
        {
            isClimbing = false;
            Gravity.isGravityEnabled = true;
            return;
        }

        Gravity.isGravityEnabled = false;
        isClimbing = true;

        playerBody.linearVelocity = Vector3.up * climbSpeed;
    }

    void EjectWall()
    {
        isClimbing = false;
        Gravity.isGravityEnabled = true;

        Vector3 ejectJump = wallNormal + Vector3.up * ejectJumpForce;
        playerBody.AddForce(ejectJump.normalized * ejectionForce, ForceMode.Impulse);
    }
}