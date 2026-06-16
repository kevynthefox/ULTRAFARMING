using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class Pickup : MonoBehaviour//, IPointerClickHandler,IScrollHandler
{
    public GameObject cursor;
    public float cursor_dist;
    public float max_dist,min_dist;
    
    public void click ()//PointerEventData eventData)
    {
        if (Input.GetMouseButton(2)) {
            Debug.Log ("middle Mouse Button Clicked");
            
            
        }
    }

    public void scroll()//PointerEventData eventData)
    {
        Debug.Log ("scrolling mouse");
        cursor_dist += Input.mouseScrollDelta.y;
        if (cursor_dist > max_dist) cursor_dist = max_dist;
        if (cursor_dist < min_dist) cursor_dist = min_dist;
        cursor.transform.localPosition = new Vector3(0,0, cursor_dist);
    }
}

