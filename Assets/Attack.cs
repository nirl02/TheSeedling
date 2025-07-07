using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    //bool isAttacking = false;
    float horizontalInput;
    public int attackDamage = 40;
    Gamepad gamepad;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }
    }


    public float attackRate = 1f;
    float nextAttackTime = 0f;
    public void Update()
    {
        // horizontalInput = Input.GetAxis("Horizontal");
        // FlipSprite();


        if (Input.GetMouseButtonDown(0) || (gamepad != null && gamepad.buttonEast.isPressed))
        {
            if (Time.time >= nextAttackTime)
            {
                Attacking();
                nextAttackTime = Time.time + 0f / attackRange;
            }
        }
        else
        {
            //isAttacking = false;
            animator.SetBool("isAttacking", false);
        }


    }


    public void Attacking()
    {
        Debug.Log("Attacking");
        //isAttacking = true;
        animator.SetBool("isAttacking", true);

        //Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //Schaden
        foreach (Collider enemy in hitEnemies)
        {
            //  float type = TypeChart.GetEffectivness()
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log(enemy.name);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;


        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

