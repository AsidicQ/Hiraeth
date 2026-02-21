using UnityEngine;

public class SprintingFOV : MonoBehaviour
{
    [Tooltip("The FOV to set when sprinting.")]
    public float FOV_increaseRate = 5f;
    public float FOV_transitionSpeed = 5f;
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
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, (defaultFOV + FOV_increaseRate), FOV_transitionSpeed);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, FOV_transitionSpeed);
        }
    }
}
