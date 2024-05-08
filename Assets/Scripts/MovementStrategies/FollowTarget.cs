using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MovementStrategy
{
    [HideInInspector]
    public new Vector3 Direction=Vector3.zero;
    [SerializeField]
    public Transform Target;
    public Vector3 GetDir()
    {
        return GetVector();
    }
    void Update()
    {
        Direction = GetVector();
        Debug.Log("followTarDir" + Direction);
    }

    private Vector3 GetVector()
    {
        if (Target == transform)
            Target = null;
        if (Target == null)
            return Vector3.zero;
        Vector3 newPos = Vector3.MoveTowards(transform.position, Target.transform.position, 1f);
        Vector3 offset = newPos - transform.position;
        return offset;
    }
}
