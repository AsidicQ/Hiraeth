using UnityEngine;

public class MeleeDetection : MonoBehaviour
{
    private Chakram chakramScript;
    public static bool canMelee = false;
    public LayerMask enemyLayer;

    void Start()
    {
        chakramScript = GetComponent<Chakram>();
    }

    private void Update()
    {
        if (chakramScript.isHolding)
        {
            DetectMelee();
        }
        else
        {
            canMelee = false;
        }
    }

    public void DetectMelee()
    {
        if (chakramScript.isHolding)
        {
            Ray meleeRay = new Ray(transform.position, transform.forward);
            
            if (Physics.Raycast(meleeRay, out RaycastHit hitInfo, chakramScript.meleeDetectionRange, enemyLayer))
            {
                canMelee = true;
            }
            else
            {
                canMelee = false;
            }
        }
    }
}
