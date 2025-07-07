using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    private Animator animator;
    public CharacterController controller;
    [SerializeField] private float m_RunSpeed = 10;

    public ThirdPersonMovement(CharacterController controller)
    {
        this.controller = controller;
    }

    public Transform cam;

    public ThirdPersonMovement(Transform cam)
    {
        this.cam = cam;
    }

    public float speed = 6f;

    // Neue Variable für Steuerungsdeaktivierung
    private bool isControlEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        EnablePlayerControl();
        Debug.Log(IsControlEnabled());
    }

    public void SetRunSpeed(float speed)
    {
        m_RunSpeed = speed;
    }

    // Methoden zur Steuerung der Spielerkontrolle
    public void DisablePlayerControl()
    {
        isControlEnabled = false;

        // Stoppe alle Bewegungen
        if (animator != null)
        {
            animator.SetFloat("xVelocity", 0f);
            animator.SetFloat("zVelocity", 0f);
        }
    }

    public void EnablePlayerControl()
    {
        isControlEnabled = true;
    }

    public bool IsControlEnabled()
    {
        return isControlEnabled;
        
    }

    public Collider coll;

    /*void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;
    }*/

    // Disables gravity on all rigidbodies entering this collider.
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Vector3 moveDir;

    // Update is called once per frame
    void Update()
    {
        // Frühe Rückkehr wenn Steuerung deaktiviert ist
        if (!isControlEnabled)
        {
            // Setze Animator-Parameter auf 0 wenn deaktiviert
            if (animator != null)
            {
                animator.SetFloat("xVelocity", 0f);
                animator.SetFloat("zVelocity", 0f);
            }
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = Vector3.zero;

        if (direction.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //  transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }


        animator.SetFloat("xVelocity", horizontal);
        animator.SetFloat("zVelocity", vertical);

        // Flip sprite for left movement
        var spriteTransform = animator.transform;
        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.z))
        {
            spriteTransform.localScale = new Vector3(moveDir.x < 0 ? -1 : 1, 1, 1);
        }
    }
    /*void OnTriggerEnter(Collider other)
       {
           if (other.attachedRigidbody)
               other.attachedRigidbody.useGravity = false;
       }*/
}