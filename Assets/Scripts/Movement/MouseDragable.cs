using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragable : MonoBehaviour
{
    Vector3 pointScreen;
    Vector3 offset;

    private void OnMouseDown()
    {
        pointScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pointScreen.z));
    }
    private void OnMouseDrag()
    {
        Debug.Log("Mouse Drag");
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pointScreen.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
        transform.position = currentPosition+ offset;

    }
}
