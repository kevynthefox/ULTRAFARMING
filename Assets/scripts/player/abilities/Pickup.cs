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
    
   /* public void click ()//PointerEventData eventData)
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

                if (hit.transform.TryGetComponent(out product_data_holder product))
                {
                    if (product.transform.parent != null)
                    {
                        if (product.transform.parent.TryGetComponent(out seed_bag_logic bag_log))
                        {
                            bag_log.bag_remove(hit.transform.gameObject);
                        }
                    }
                }
                
                
                //StartCoroutine(mover(hit.transform.gameObject));

                obj = hit.transform.gameObject;
            }
        }
    }

    public IEnumerator mover(GameObject obj)
    {
        while (this.enabled == true)
        {
            if (Input.GetMouseButton(2))
            {
                //obj.transform.position = Vector3.MoveTowards(obj.transform.position,cursor.transform.position, Time.deltaTime * move_speed * Vector3.Distance(obj.transform.position,cursor.transform.position));
                //obj.transform.localEulerAngles = cursor.transform.forward;
                if (obj.transform.parent != null)
                {
                    previous_parent = obj.transform.parent;
                }
                obj.transform.parent = cursor.transform;
                obj.transform.localPosition = Vector3.zero;
                
                
                if (obj.CompareTag("employee_management"))
                {
                    Debug.Log("picking up an employee management object");
                    if (obj.transform.parent.TryGetComponent(out area_designator designator))//should add functionality for if the area designator is the object, not the object's parent
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
                
                yield return new WaitForEndOfFrame();
            }
            else
            {
                if (had_gravity == true || obj.TryGetComponent(out cropGrowth growth))
                {
                    if (obj.TryGetComponent(out Rigidbody rb))
                    {
                        rb.useGravity = true;
                    }
                }

                had_gravity = false; // this part above is to add an exception so that crops harvested by a scythe which float in the air to give the player a chance to collect them, will fall down if you let go of them. meaning they can be replanted
                yield break;
            }
        }
    }*/

    public void move(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //this is the part for getting the object

            #region get_obj

            

            
            Physics.Raycast(viewer.transform.position,viewer.transform.forward, out RaycastHit hit,cursor_dist+pickup_margin,acceptable_layers);

            if (hit.transform != null)
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.distance <= cursor_dist + pickup_margin)
                {
                
                    /*if (hit.transform.TryGetComponent(out Rigidbody rb))
                    {
                        
                        
                        if (rb.useGravity == true) had_gravity = true; //doing it like this and not hadgravity = rb.usegravity, because that second part can change

                        if (had_gravity == true)
                        {
                            rb.useGravity = false;
                        }
                    }*/

                    /*if (hit.transform.TryGetComponent(out product_data_holder product))
                    {
                        if (product.transform.parent != null)
                        {
                            if (product.transform.parent.TryGetComponent(out seed_bag_logic bag_log))
                            {
                                bag_log.bag_remove(hit.transform.gameObject);
                                if (hit.transform.gameObject.TryGetComponent(out Rigidbody product_rb))
                                {
                                    product_rb.useGravity = false;
                                    had_gravity = true;
                                }
                                previous_parent = null;
                            }
                        }
                    }*/
                
                
                    //StartCoroutine(mover(hit.transform.gameObject));

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
                }
                else
                {
                    previous_parent = null;
                }

                

                obj.transform.parent = cursor.transform;
                obj.transform.localPosition = Vector3.zero;
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
            }
            #endregion
        }

        if (context.canceled)
        {
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

