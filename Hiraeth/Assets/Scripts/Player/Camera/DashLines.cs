using UnityEngine;

public class DashLines : MonoBehaviour
{
    public Transform cameraTransform;

    private void LateUpdate()
    {
        float yRotation = cameraTransform.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0f, yRotation + 180, 0f);
    }
}
