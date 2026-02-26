using UnityEngine;

public class HandSway : MonoBehaviour
{
    [Header("Position Sway")]
    public float positionClamp = 0.89f;
    public float positionSmoothing = 5f;

    [Header("Rotation Sway")]
    public float rotationClamp = 5f;
    public float rotationSmoothing = 7f;

    [Header("Input Smoothing")]
    public float inputSmooth = 10f;
    public float inputDeadZone = 0.02f;

    private Vector3 origin;
    private Quaternion originRot;
    private Vector2 smoothedInput;

    void Start()
    {
        origin = transform.localPosition;
        originRot = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 rawInput = new Vector2(mouseX, mouseY);
        smoothedInput = Vector2.Lerp(smoothedInput, rawInput, Time.deltaTime * inputSmooth);

        if (Mathf.Abs(smoothedInput.x) < inputDeadZone) smoothedInput.x = 0f;
        if (Mathf.Abs(smoothedInput.y) < inputDeadZone) smoothedInput.y = 0f;

        float posX = Mathf.Clamp(-mouseX, -positionClamp, positionClamp);
        float posY = Mathf.Clamp(-mouseY, -rotationClamp, rotationClamp);
        Vector3 targetPos = new Vector3(posX, posY, 0) + origin;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * positionSmoothing);

        float rotX = Mathf.Clamp(-smoothedInput.y * rotationClamp, -rotationClamp, rotationClamp);
        float rotY = Mathf.Clamp(-smoothedInput.x * rotationClamp, -rotationClamp, rotationClamp);
        Quaternion targetRot = Quaternion.Euler(rotX, rotY, 0) * originRot;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRot,
            Time.deltaTime * rotationSmoothing);
    }
}