using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Mouse Settings")]
    [Range(0.1f, 5f)] public float sensitivityAmount;
    [Range(40, 60)] public float smoothAmount = 45;

    private float xRotation;
    private float yRotation;

    private Vector2 currentRotation;

    public Transform orientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        // Raw input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityAmount;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        currentRotation.x = Mathf.Lerp(currentRotation.x, xRotation, smoothAmount * Time.deltaTime);
        currentRotation.y = Mathf.Lerp(currentRotation.y, yRotation, smoothAmount * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
}

