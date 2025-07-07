using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    float horizontalInput;
    public int attackDamage = 40;
    Gamepad gamepad;

    public float attackRate = 1f;
    float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || (gamepad != null && gamepad.buttonEast.isPressed))
        {
            if (Time.time >= nextAttackTime)
            {
                Attacking();
                nextAttackTime = Time.time + 1f / attackRate; // Korrigiert: war 0f / attackRange
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    public void Attacking()
    {
        Debug.Log("Player attacking");

        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
        }

        // Sicherheitscheck für attackPoint
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint is not assigned!");
            return;
        }

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Schaden verursachen
        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider == null) continue;

            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log($"Hitting enemy: {enemy.name}");
                enemy.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning($"Object {enemyCollider.name} has no Enemy component but is on enemy layer");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}