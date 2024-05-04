using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void Move(Transform transform, float speed, Vector3 direction);
}
