using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Headbob Settings")]
    public bool enable = true;
    [SerializeField, Range(0f, 0.2f)] private float walkAmplitude = 0.015f;
    [SerializeField, Range(0f, 0.2f)] private float runAmplitude = 0.03f;

    [SerializeField, Range(0f, 30f)] private float walkFrequency = 6;
    [SerializeField, Range(0f, 30f)] private float runFrequency = 9;

    [SerializeField] private float tiltAmount = 1.5f;
    [SerializeField] private float tiltSpeed = 5f;

    [SerializeField] private float toggleSpeed = 0.1f;
    [SerializeField] private float cameraReturnSpeed = 12f;

    private float bobCycle = 0f;
    private float smoothedSpeed = 0f;
    [SerializeField] private float speedSmooth = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder = null;
    public Rigidbody controller;
    public Movement movement;

    [Header("Dash Ability")]
    [HideInInspector] public float dashBobAmount = 0f;
    private float dashBlend;
    [HideInInspector] public bool isDashing = false;

    private Vector3 startLocalPos;

    void Start()
    {
        if (cameraHolder == null)
        {
            Debug.LogError("CameraHolder not assigned in HeadbobController!");
            enabled = false;
            return;
        }

        startLocalPos = cameraHolder.localPosition;
    }

    void LateUpdate()
    {
        if (!enable || controller == null || movement == null)
            return;

        //Find Movement Speed
        Vector3 horizontalVelocity = new Vector3(controller.linearVelocity.x, 0f, controller.linearVelocity.z);

        float targetSpeed = (movement.moveSpeed > 0.1f && movement.grounded) ? horizontalVelocity.magnitude : 0f;
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, speedSmooth * Time.deltaTime);

        if (bobCycle > Mathf.PI * 2f)
            bobCycle -= Mathf.PI * 2f;

        //Walking or Running Bob
        Vector3 walkOffset = Vector3.zero;
        float amplitude = (smoothedSpeed > movement.sprintSpeed) ? runAmplitude : walkAmplitude;
        float frequency = (smoothedSpeed > movement.sprintSpeed) ? runFrequency : walkFrequency;
        float tilt = 0f;

        bobCycle += smoothedSpeed * Time.deltaTime;

        if (!isDashing && smoothedSpeed >= toggleSpeed && movement.grounded)
        {
            walkOffset = new Vector3(
                Mathf.Cos(bobCycle * frequency / 2f) * amplitude * 2f,
                Mathf.Sin(bobCycle * frequency) * amplitude, 0f);
            tilt = Mathf.Sin(bobCycle * frequency) * tiltAmount;
        }

        //Rotation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, tilt);
        cameraHolder.localRotation = Quaternion.Lerp(cameraHolder.localRotation, targetRotation,
        tiltSpeed * Time.deltaTime);

        // Dash Bob
        dashBlend = Mathf.Lerp(dashBlend, dashBobAmount, 15f * Time.deltaTime);
        Vector3 dashOffset = new Vector3(0f, dashBlend, 0f);

        Vector3 targetPos = startLocalPos + walkOffset + dashOffset;
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPos, 
            cameraReturnSpeed * Time.deltaTime);
    }

    public void ResetBobCycle()
    {
        bobCycle = 0f;
        smoothedSpeed = 0f;
    }
}