using System;
using System.Collections;
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
    public float fadeDuration = 0.2f;

    private void Start()
    {
        if (hitMarkerImage != null)
            hitMarkerImage.enabled = false;
    }

    public void TriggerHitMarker()
    {
        StopAllCoroutines();
        StartCoroutine(HitMarkerCoroutine());
        OnHitMarker?.Invoke();
    }
    
    public IEnumerator HitMarkerCoroutine()
    {
        hitMarkerImage.enabled = true;
        Color originalColor = hitMarkerImage.color;

        float elapsedTime = 0f;
        while (elapsedTime < hitmarkerDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            originalColor.a = newAlpha;
            hitMarkerImage.color = originalColor;
            yield return null;
        }

        yield return new WaitForSeconds(hitmarkerDuration); 

        elapsedTime = 0;
        while (elapsedTime < hitmarkerDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            originalColor.a = alpha;
            hitMarkerImage.color = originalColor;
            yield return null;
        }

        originalColor.a = 0f;
        hitMarkerImage.color = originalColor;
        hitMarkerImage.enabled = false;
    }
}