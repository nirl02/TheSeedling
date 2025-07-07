using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock;
public class MyPlayerScript : MonoBehaviour

    
{
    ThirdPersonMovement moveScript;
    ThirdPersonDash moveScript2;
    MeleeAttack attackScript;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovement>();
        moveScript2 = GetComponent<ThirdPersonDash>();
        attackScript = GetComponent<MeleeAttack>();
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return; // No gamepad connected.
        }

        if (gamepad.rightTrigger.isPressed)
        {
            moveScript2 = GetComponent<ThirdPersonDash>();
        }

        Vector3 move = gamepad.leftStick.ReadValue();
        {
            moveScript = GetComponent<ThirdPersonMovement>();
            //Debug.Log("left");
        }

        if (gamepad.buttonEast.isPressed)
        {
            //Debug.Log("Kreis gedrückt");

           // attackScript.Update;
        }

        if (gamepad.rightShoulder.isPressed)
        {
            //Debug.Log("R1 gedrückt");
            // Beispielaktion: Dash oder Block etc.
        }

    }
}