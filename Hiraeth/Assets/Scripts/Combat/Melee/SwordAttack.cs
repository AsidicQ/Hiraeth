using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float sphereRange;
    public int attackDamage;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Input")]
    public KeyCode attackKey;

    [Header("Animations")]
    private Animator animator;
    private int comboCount;
    public float attackCooldown = 1f;
    private bool isAttacking = false;

    [Header("References")]
    public HitMarkers hitMarkers;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            comboCount = (comboCount + 1) % 3; // Cycle through combo attacks (0, 1, 2)
            StartCoroutine(MeleeAttack());

            float comboResetTime = 1.5f; // Time to reset combo if no attack is made
            if (comboCount > 0)
            {
                comboResetTime -= Time.deltaTime;
                if (comboResetTime <= 0)
                {
                    comboCount = 0; // Reset combo if time runs out
                }
            }
        }
    }

    public IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetInteger("ComboCount", comboCount);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, sphereRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            hitMarkers.TriggerHitMarker();
            Debug.Log("Hit " + enemy.name);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, sphereRange);
    }

    public void Initialize(HitMarkers hitMarkers)
    {
        this.hitMarkers = hitMarkers;
    }
}