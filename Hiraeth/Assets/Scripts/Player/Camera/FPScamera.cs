using UnityEngine;

public class FPScamera : MonoBehaviour
{
    public Camera mainCamera;

    void LateUpdate()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}

