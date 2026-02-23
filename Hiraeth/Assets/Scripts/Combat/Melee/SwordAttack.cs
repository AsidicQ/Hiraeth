using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange;
    public int attackDamage;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Input")]
    public KeyCode attackKey;

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            MeleeAttack();
        }
    }

    public void MeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<DummyMovement>(out DummyMovement dummyHealth))
            {
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}