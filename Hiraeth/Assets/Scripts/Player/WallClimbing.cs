using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody))]
public class WallClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    public float climbHeight = 8f;
    public float climbActivationThreshold = 1f;
    public float speedToClimbNeeded = 2f;
    public static bool isClimbing = false;

    [Header("Ejection Settings")]
    public float ejectionForce = 5f;
    private Transform wallToEjectFrom;

    [Header("References")]
    private Movement movementScript;
    private Rigidbody playerBody;

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
    }

    void DetectingClimb()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(movementScript.jumpKey))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, climbActivationThreshold))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                {
                    ClimbForce();
                    wallToEjectFrom = hit.transform;
                }
                else
                    isClimbing = false;
            }
        }
    }

    void ClimbForce()
    {
        isClimbing = true;
        playerBody.AddForce(transform.up * climbSpeed, ForceMode.Impulse);

        if (Input.GetKeyDown(ejectKey))
        {
            EjectWall();
        }
    }

    void EjectWall()
    {
        isClimbing = false;

        Vector3 ejectDirection = (transform.position - wallToEjectFrom.position).normalized;
        playerBody.AddForce(ejectDirection * ejectionForce, ForceMode.Impulse);
    }
}