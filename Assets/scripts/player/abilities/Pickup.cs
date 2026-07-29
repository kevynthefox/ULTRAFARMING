using System;
using System.Collections;
using Unity.Cinemachine;
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

    public float move_speed;

    public GameObject obj;
    public Transform previous_parent;

    public int rotation_state;

    public bool grabbing;
    public current_hands_cursor currenthands;
   
    public void move(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabbing = true;
            //this is the part for getting the object

            #region get_obj

            

            
            Physics.Raycast(viewer.transform.position,viewer.transform.forward, out RaycastHit hit,cursor_dist+pickup_margin,acceptable_layers);

            if (hit.transform != null)
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.distance <= cursor_dist + pickup_margin)
                {

                    obj = hit.transform.gameObject;
                }
            }
            #endregion

            #region move_obj

            

            
            if (obj != null)
            {
                if (obj.TryGetComponent(out product_data_holder prod_data))
                {
                    prod_data.being_held = true;
                }
                
                //this is the moving part
                if (obj.transform.parent != null)
                {
                    previous_parent = obj.transform.parent;
                    if (obj.transform.parent.TryGetComponent(out seed_bag_logic bag_logic))
                    {
                        bag_logic.bag_remove(obj);
                        previous_parent = null;
                    }
                    if (obj.transform.parent.TryGetComponent(out cart_logic cart_log))
                    {
                        previous_parent = null;
                    }
                }
                else
                {
                    previous_parent = null;
                }


                if (perk_logic.current.perk6)
                {
                    currenthands.StartCoroutine(currenthands.puppet());
                }
                else
                {
                    obj.transform.parent = cursor.transform;
                    obj.transform.localPosition = Vector3.zero;
                }

                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                }


                if (obj.CompareTag("employee_management"))
                {
                    Debug.Log("picking up an employee management object");
                    if (obj.transform.parent.TryGetComponent(out area_designator designator)) //should add functionality for if the area designator is the object, not the object's parent
                    {
                        designator.display_distance();
                    }
                }

                if (obj.CompareTag("growing_crop"))
                {
                    //Debug.Log("picking up a growing crop");
                    if (obj.TryGetComponent(out cropGrowth cropgrowth))
                    {
                        //Debug.Log("it had crop growth");
                        if (cropgrowth.started_growth == true)
                        {
                            //Debug.Log("it was growing");
                            obj.tag = "finished_crop";
                        }
                    }
                }

                if (obj.CompareTag("sellable"))
                {
                    if (obj.TryGetComponent(out cropGrowth growth))
                    {
                        if (perk_logic.current.perk4)
                        {
                            growth.hydrate(-1);
                        }
                        if (perk_logic.current.perk5)
                        {
                            growth.hydrate(1);
                        }
                    }
                }
            }
            #endregion
        }

        if (context.canceled)
        {
            grabbing = false;
            #region release_obj
            
            if (obj != null)
            {
                if (obj.TryGetComponent(out product_data_holder prod_data))
                {
                    prod_data.being_held = false;
                }
                
                /*if (had_gravity == true || obj.TryGetComponent(out cropGrowth growth))
                {
                    if (obj.TryGetComponent(out Rigidbody rb))
                    {
                        rb.useGravity = true;
                    }
                }*/
                
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                }

                //had_gravity = false; // this part above is to add an exception so that crops harvested by a scythe which float in the air to give the player a chance to collect them, will fall down if you let go of them. meaning they can be replanted

                
                obj.transform.parent = previous_parent;
                
                
                previous_parent = null;
                obj = null;
            }
            #endregion
        }


}

    public void scroll(InputAction.CallbackContext context) 
    {
        var cachedInput = context.ReadValue<Vector2>();
        float y_plus = cachedInput.y;
        //Debug.Log ("scrolling mouse");

        if (Input.GetKey(KeyCode.Alpha1)) rotation_state = 1;
        if (Input.GetKey(KeyCode.Alpha2)) rotation_state = 2;
        if (Input.GetKey(KeyCode.Alpha3)) rotation_state = 3;
        if (Input.GetKey(KeyCode.Alpha4)) rotation_state = 0;//would use keyDOWN but they already have to be scrolling so that makes this tricky to time
        if (Input.GetKey(KeyCode.Alpha5)) cursor.transform.localEulerAngles = Vector3.zero;
        
        
        if (rotation_state == 0 ) cursor_dist += y_plus;// Input.mouseScrollDelta.y;
        if (rotation_state == 1) cursor.transform.localEulerAngles = new Vector3(cursor.transform.localEulerAngles.x + y_plus, cursor.transform.localEulerAngles.y, cursor.transform.localEulerAngles.z);
        if (rotation_state == 2) cursor.transform.localEulerAngles = new Vector3(cursor.transform.localEulerAngles.x, cursor.transform.localEulerAngles.y + y_plus, cursor.transform.localEulerAngles.z);
        if (rotation_state == 3) cursor.transform.localEulerAngles = new Vector3(cursor.transform.localEulerAngles.x, cursor.transform.localEulerAngles.y, cursor.transform.localEulerAngles.z + y_plus);
        
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

