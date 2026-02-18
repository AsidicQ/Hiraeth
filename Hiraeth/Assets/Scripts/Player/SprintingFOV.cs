using UnityEngine;

public class SprintingFOV : MonoBehaviour
{
    [Tooltip("The FOV to set when sprinting.")]
    public float TargetFOV = 80f;
    public float FOVTransitionSpeed = 5f;
    private float defaultFOV;

    [Header("References")]
    public Camera playerCamera;
    private Movement playerMovement;


    void Start()
    {
        defaultFOV = playerCamera.fieldOfView;
        playerMovement = GetComponent<Movement>();
    }

    void Update()
    {
        if (playerMovement.state == Movement.MovementState.Sprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, TargetFOV, Time.deltaTime * FOVTransitionSpeed);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, Time.deltaTime * FOVTransitionSpeed);
        }
    }
}
