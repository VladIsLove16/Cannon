using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStrategy : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Direction { get; set; }
    public Vector3 GetDir()
    {
        return Direction;
    }
    void Update()
    {
        Direction = Vector3.forward;
    }
}
