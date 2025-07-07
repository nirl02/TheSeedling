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
    private bool isAttacking = false;
    private float attackCooldown = 2f;
    private float nextAttackTime = 0f;


    /* Besiegen des Gegners */
    public delegate void EnemyKilled(Enemy enemy);

    /* Event: Ein Gegner wird besiegt */
    public event EnemyKilled g_onEnemyKilled;

    /**
    * Zu Begin Lebensanzeige initialisieren und Waffe spawnen
    */
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        animator.SetBool("isAttacking", false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy says Ouch!");

        if (currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        // Benachrichtige den Spieler über den Tod des Gegners
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnEnemyKilled();
        }

        // Event für andere Systeme auslösen (falls noch andere darauf hören)
        g_onEnemyKilled?.Invoke(this);

        Destroy(transform.root.gameObject);
    }

    /* Waypointtraveler des Gegeners */
    private WaypointsTraveler g_waypointsTraveler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            g_player = other.GetComponent<Player>();
            PlayerInRange(g_player);
            Debug.Log("Enemy saw player");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExitRange();

        }

    }

    /**
    * Spieler betritt den Sichtbereich des Gegners und bleibt stehen.
    * @param p - Spieler im Sichbereich.
    */
    public void PlayerInRange(Player p)
    {

        g_waypointsTraveler = transform.root.GetComponent<WaypointsTraveler>();

        // Wenn es einen WaypointTraveler gibt, wird dieser angehalten.
        if (g_waypointsTraveler != null)
        {
            g_waypointsTraveler.Move(false);
        }

        playerInRange = true;


    }

    public void DealDamage()
    {
        if (g_player != null)
        {
            g_player.OnHit(dmg);
        }
    }



    /**
    * Spieler verlässt den Sichtbereich des Gegners und dieser lauft weiter.
*/
    public void PlayerExitRange()
    {
        // Wenn es einen WaypointTraveler gibt, wird dieser fortgesetzt.
        if (g_waypointsTraveler != null)
        {
            g_waypointsTraveler.Move(true);
        }

        playerInRange = false;
        g_player = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (playerInRange)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackCooldown;
            }
        }

        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;


        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);


        animator.SetFloat("xVelocity", horizontalVelocity.x);
        animator.SetFloat("zVelocity", horizontalVelocity.z);

        // Flip the sprite depending on movement direction (left/right)
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))  // prioritize horizontal movement
        {
            Vector3 scale = transform.localScale;
            scale.x = velocity.x < 0 ? -1 : 1;
            transform.localScale = scale;
        }



    }



    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}