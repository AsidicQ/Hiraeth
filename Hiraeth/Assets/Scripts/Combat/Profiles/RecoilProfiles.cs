using UnityEngine;

[CreateAssetMenu(fileName = "NewRecoilProfiles", menuName = "Weapons/Recoil Profile")]
public class RecoilProfiles : ScriptableObject
{
    [Header("Recoil Settings")]
    [Range(-10.0f, 10.0f)] public float recoilX = 1.0f;
    [Range(-10.0f, 10.0f)] public float recoilY = 1.0f;
    [Range(0, 0.7f)] public float kickBack = 0.1f;

    [Header("Recoil Recovery")]
    public float snapAmount = 10.0f;
    public float returnSpeed = 5.0f;
}

