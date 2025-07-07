using System;
using UnityEngine;
using WaypointsFree;

public class Enemy : MonoBehaviour
{
    /* Der Spieler */
    private Player g_player;

    public int maxHealth = 100;
    public int dmg = 25;
    int currentHealth;

    [SerializeField] private String element_type;

    /* Animation Parameters */
    private Animator animator;
    private Vector3 lastPosition;
    private Vector3 velocity;

    /*Attack Parameters*/
    private bool playerInRange = false;
    private float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    /* Besiegen des Gegners */
    public delegate void EnemyKilled(Enemy enemy);
    public event EnemyKilled g_onEnemyKilled;

    /* Waypointtraveler des Gegeners */
    private WaypointsTraveler g_waypointsTraveler;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        animator.SetBool("isAttacking", false);
    }

    public void TakeDamage(int damage)
    {
        // Sicherheitscheck: Nur Schaden nehmen wenn der Gegner noch lebt
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        Debug.Log($"Enemy {gameObject.name} takes {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died");

        // Stoppe alle Bewegungen und Animationen
        if (g_waypointsTraveler != null)
        {
            g_waypointsTraveler.Move(false);
        }

        // Stoppe Animator
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Deaktiviere Collider um weitere Interaktionen zu verhindern
        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Benachrichtige den Spieler über den Tod des Gegners
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnEnemyKilled();
        }

        // Event für andere Systeme auslösen (falls noch andere darauf hören)
        try
        {
            g_onEnemyKilled?.Invoke(this);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error invoking enemy killed event: {e.Message}");
        }

        // Verzögertes Zerstören um sicherzustellen, dass alle Events abgearbeitet wurden
        StartCoroutine(DestroyAfterDelay());
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        // Kurz warten um sicherzustellen, dass alle Events und Animationen abgeschlossen sind
        yield return new WaitForSeconds(0.1f);

        // Nur das eigene GameObject zerstören, NICHT das Root-GameObject
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignoriere Trigger wenn der Gegner bereits tot ist
        if (currentHealth <= 0)
            return;

        if (other.CompareTag("Player"))
        {
            g_player = other.GetComponent<Player>();
            if (g_player != null)
            {
                PlayerInRange(g_player);
                Debug.Log($"Enemy {gameObject.name} saw player");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ignoriere Trigger wenn der Gegner bereits tot ist
        if (currentHealth <= 0)
            return;

        if (other.CompareTag("Player"))
        {
            PlayerExitRange();
        }
    }

    public void PlayerInRange(Player p)
    {
        // Sicherheitscheck
        if (currentHealth <= 0)
            return;

        g_waypointsTraveler = transform.root.GetComponent<WaypointsTraveler>();

        if (g_waypointsTraveler != null)
        {
            g_waypointsTraveler.Move(false);
        }

        playerInRange = true;
    }

    public void DealDamage()
    {
        // Sicherheitscheck: Nur Schaden verursachen wenn der Gegner noch lebt
        if (currentHealth <= 0 || g_player == null)
            return;

        g_player.OnHit(dmg);
    }

    public void PlayerExitRange()
    {
        // Sicherheitscheck
        if (currentHealth <= 0)
            return;

        if (g_waypointsTraveler != null)
        {
            g_waypointsTraveler.Move(true);
        }

        playerInRange = false;
        g_player = null;
    }

    void Update()
    {
        // Stoppe alle Updates wenn der Gegner tot ist
        if (currentHealth <= 0)
            return;

        // Attack Logic
        if (Time.time >= nextAttackTime)
        {
            if (playerInRange && g_player != null)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        // Movement und Animation Logic
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        if (animator != null && animator.enabled)
        {
            animator.SetFloat("xVelocity", horizontalVelocity.x);
            animator.SetFloat("zVelocity", horizontalVelocity.z);
        }

        // Sprite flipping
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))
        {
            Vector3 scale = transform.localScale;
            scale.x = velocity.x < 0 ? -1 : 1;
            transform.localScale = scale;
        }
    }

    public void OnAttackAnimationEnd()
    {
        // Sicherheitscheck
        if (currentHealth <= 0)
            return;

        // Hier könntest du weitere Logik für das Ende der Attacke hinzufügen
    }
}