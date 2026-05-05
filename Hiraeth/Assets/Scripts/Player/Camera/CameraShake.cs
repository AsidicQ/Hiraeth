using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator CameraShakeProcess(float shakeDuration, float shakeAmount)
    {
        Vector3 initialPos = transform.localPosition;
        float runningTime = 0f;

        while (runningTime < shakeDuration)
        {
            runningTime += Time.deltaTime;

            float strength = Mathf.Lerp(shakeAmount, 0f, runningTime / shakeDuration);

            Vector3 offset = Random.insideUnitSphere * strength;
            offset.z = 0;

            transform.localPosition = initialPos + offset;
            yield return null;
        }

        transform.localPosition = initialPos;
    }
}