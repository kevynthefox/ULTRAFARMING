using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class Pickup : MonoBehaviour//, IPointerClickHandler,IScrollHandler
{
    public GameObject cursor;
    public GameObject viewer;
    public float cursor_dist;
    public float max_dist,min_dist;
    public float pickup_margin; //the margin of error for if the object can be picked up
    public LayerMask acceptable_layers;

    public bool had_gravity;
    
    public void click ()//PointerEventData eventData)
    {
        Debug.Log("click");
        
        Physics.Raycast(viewer.transform.position,viewer.transform.forward, out RaycastHit hit,cursor_dist+pickup_margin,acceptable_layers);

        if (hit.transform != null)
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.distance <= cursor_dist + pickup_margin)
            {
                
                if (hit.transform.TryGetComponent(out Rigidbody rb))
                {
                    if (rb.useGravity == true) had_gravity = true; //doing it like this and not hadgravity = rb.usegravity, because that second part can change

                    if (had_gravity == true)
                    {
                        rb.useGravity = false;
                    }
                }
                StartCoroutine(mover(hit.transform.gameObject));

                
            }
        }
    }

    public IEnumerator mover(GameObject obj)
    {
        while (this.enabled == true)
        {
            if (Input.GetMouseButton(2))
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position,cursor.transform.position, pickup_margin);

                if (obj.CompareTag("employee_management"))
                {
                    Debug.Log("picking up an employee management object");
                    if (obj.transform.parent.TryGetComponent(out area_designator designator))//should add functionality for if the area designator is the object, not the object's parent
                    {
                        designator.display_distance();
                    }
                }
                
                yield return new WaitForEndOfFrame();
            }
            else
            {
                if (had_gravity == true) obj.GetComponent<Rigidbody>().useGravity = true;
                had_gravity = false;
                yield break;
            }
        }
    }

    public void scroll(InputAction.CallbackContext context) 
    {
        var cachedInput = context.ReadValue<Vector2>();
        float y_plus = cachedInput.y;
        //Debug.Log ("scrolling mouse");
        cursor_dist += y_plus;// Input.mouseScrollDelta.y;
        if (cursor_dist > max_dist) cursor_dist = max_dist;
        if (cursor_dist < min_dist) cursor_dist = min_dist;
        cursor.transform.localPosition = new Vector3(0,0, cursor_dist);
    }
    
    
    public void button_press ()//PointerEventData eventData)
    {
        
        
        Physics.Raycast(viewer.transform.position,viewer.transform.forward, out RaycastHit hit,cursor_dist+pickup_margin,5);//ui is layer 5

        if (hit.transform != null)
        {
            if (hit.distance <= cursor_dist + pickup_margin)
            {
                //Debug.Log("press");
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.CompareTag("button_world"))
                {

                    if (hit.transform.gameObject.TryGetComponent(out button_translator button))
                    {
                        button.clicked_on();
                    }


                }
            }
        }
    }
}

