using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Vector2 Movement {  get; set; }
    [SerializeField]
    public float movementSpeed;

    private void FixedUpdate()
    {
        Vector2 newPos = transform.localPosition + new Vector3(movementSpeed * Time.deltaTime * Movement.x, movementSpeed * Time.deltaTime * Movement.y, 0);
        transform.localPosition = newPos;
    }
}
