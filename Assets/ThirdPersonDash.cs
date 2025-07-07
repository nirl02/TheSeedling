using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ThirdPersonDash : MonoBehaviour
{

    ThirdPersonMovement moveScript;
    Gamepad gamepad;

    public float dashSpeed;
    public float dashTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovement>();
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || (gamepad != null && gamepad.rightTrigger.isPressed))
        {
            StartCoroutine(Dash());
            Debug.Log("Dashing");
        }
    }


    private IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveScript.controller.Move(moveScript.moveDir * dashSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * dashSpeed);
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
            yield return null;
        }
    }
}
