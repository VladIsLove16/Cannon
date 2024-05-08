using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerInput : MonoBehaviour
{
    [SerializeField]
    public InputActionReference pointerPosition;
    static Vector3 mousePosition;
    private void Update()
    {
        mousePosition = pointerPosition.action.ReadValue<Vector3>();
    }
    //public static Vector2 GetPointerInput()
    //{
    //    return Camera.main.ScreenToWorldPoint(mousePosition);
    //}
    public static Vector3 GetPointerInput()
    {
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
    public static Vector2 GetPointerInputVector2()
    {
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
