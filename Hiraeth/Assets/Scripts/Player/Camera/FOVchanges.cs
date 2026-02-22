using UnityEngine;

public class SprintingFOV : MonoBehaviour
{
    [Tooltip("The FOV to set when sprinting.")]
    public float staminaIncrease = 5f;
    public float dashIncrease = 10f;
    private float defaultFOV;
    private float fovVelocity;

    [Header("References")]
    public Camera playerCamera;
    private Movement playerMovement;
    private GroundDash groundDash;


    void Start()
    {
        defaultFOV = playerCamera.fieldOfView;
        playerMovement = GetComponent<Movement>();
        groundDash = GetComponent<GroundDash>();
    }

    void Update()
    {
        float targetFOV = defaultFOV;

        if (groundDash.isDashing)
        {
            targetFOV = defaultFOV + dashIncrease;
        }
        else if (playerMovement.state == Movement.MovementState.Sprinting)
        {
            targetFOV = defaultFOV + staminaIncrease;
        }

        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, targetFOV, 
            ref fovVelocity, 0.2f);
    }
}
