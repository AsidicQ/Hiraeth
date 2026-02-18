using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Headbob Settings")]
    public bool enable = true;
    [SerializeField, Range(0f, 0.2f)] private float walkAmplitude = 0.03f;
    [SerializeField, Range(0f, 0.2f)] private float runAmplitude = 0.06f;
    [SerializeField, Range(0f, 30f)] private float walkFrequency = 2;
    [SerializeField, Range(0f, 30f)] private float runFrequency = 4;
    [SerializeField] private float toggleSpeed = 0.1f;

    private float bobCycle = 0f;
    private float smoothedSpeed = 0f;
    [SerializeField] private float speedSmooth = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder = null;
    public Rigidbody controller;
    public Movement movement;

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

        float targetSpeed = new Vector3(controller.linearVelocity.x, 0f, controller.linearVelocity.z).magnitude;
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, speedSmooth * Time.deltaTime);

        bobCycle += smoothedSpeed * Time.deltaTime;

        if (bobCycle > Mathf.PI * 2f)
            bobCycle -= Mathf.PI * 2f;

        if (smoothedSpeed < toggleSpeed || !movement.grounded)
        {
            ResetPosition();
            return;
        }

        ApplyHeadbob(smoothedSpeed);
    }

    private void ApplyHeadbob(float speed)
    {
        float amplitude = (speed > movement.sprintSpeed) ? runAmplitude : walkAmplitude;
        float frequency = (speed > movement.sprintSpeed) ? runFrequency : walkFrequency;

        Vector3 offset = new Vector3(
            Mathf.Cos(bobCycle * frequency / 2f) * amplitude * 2f,
            Mathf.Sin(bobCycle * frequency) * amplitude,
            0f
        );

        cameraHolder.localPosition = startLocalPos + offset;
    }

    private void ResetPosition()
    {
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, startLocalPos, 5f * Time.deltaTime);
    }
}

