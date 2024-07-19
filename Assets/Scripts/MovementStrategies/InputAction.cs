using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputAction : MovementStrategy
{
    [HideInInspector]
    public Vector3 Direction = Vector3.zero;
    [SerializeField]
    //InputActionReference Movement;
    public Vector3 GetDir()
    {
        return Direction;
    }
    //private Vector3 movementInput { get; set; }
    void Update()
    {
        //Direction = Movement.action.ReadValue<Vector3>();
    }
}
