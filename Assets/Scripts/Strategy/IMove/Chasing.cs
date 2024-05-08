using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : IMove
{
    public void Algorithm(UnityEngine.Transform transform)
    {
        //float dx = movementSpeed * lastDirection.x;
        //float dy = movementSpeed * lastDirection.y;
        //Vector2 newPos = transform.localPosition + new Vector3(movementSpeed * Time.deltaTime * dx, movementSpeed * Time.deltaTime * dy, 0);
        //transform.localPosition = newPos;
    }

    public void Move(UnityEngine.Transform transform, float speed, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
