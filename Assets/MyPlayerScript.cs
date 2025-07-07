using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock;
public class MyPlayerScript : MonoBehaviour

    
{
    ThirdPersonMovement moveScript;
    MeleeAttack attackScript;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovement>();
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
        }

        Vector3 move = gamepad.leftStick.ReadValue();
        {
            moveScript = GetComponent<ThirdPersonMovement>();
            //Debug.Log("left");
        }

        if (gamepad.buttonEast.isPressed)
        {
            //Debug.Log("Kreis gedr�ckt");

           // attackScript.Update;
        }

        if (gamepad.rightShoulder.isPressed)
        {
            //Debug.Log("R1 gedr�ckt");
            // Beispielaktion: Dash oder Block etc.
        }

    }
}