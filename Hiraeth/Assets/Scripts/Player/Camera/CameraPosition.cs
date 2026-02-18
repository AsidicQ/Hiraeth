using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
