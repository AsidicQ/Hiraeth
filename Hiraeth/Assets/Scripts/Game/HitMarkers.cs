using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HitMarkers : MonoBehaviour
{
    public event Action OnHitMarker;

    [Header("UI Elements")]
    public Image hitMarkerImage;
    public Color bodyDamageIndicator;
    public Color headshotDamageIndicator;
    public Color killIndicator;
    public float hitmarkerDuration = 0.5f;

    public void TriggerHitMarker()
    {
        StartCoroutine(HitMarkerCoroutine());
        OnHitMarker?.Invoke();
    }

    public IEnumerator HitMarkerCoroutine()
    {
        float elapsedTime = 0f;
        Color originalColor = hitMarkerImage.color;

        while (elapsedTime < hitmarkerDuration)
        {
            elapsedTime += Time.deltaTime;

        }

        yield return new WaitForSeconds(0.5f); 
    }
}