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
    private int randomAttackIndex;
    public float attackCooldown = 1f;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            RandomiseAnimation();
            StartCoroutine(MeleeAttack());
        }
    }

    public void RandomiseAnimation()
    {
        randomAttackIndex = Random.Range(0, 2);
    }

    public IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetInteger("RandomAttackIndex", randomAttackIndex);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, sphereRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<DummyMovement>(out DummyMovement dummyHealth))
            {
            }
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
}