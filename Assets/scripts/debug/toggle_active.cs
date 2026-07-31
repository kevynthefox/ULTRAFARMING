using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class toggle_active : MonoBehaviour
{
    public GameObject target;

    public bool uncouple_mouse; //this is really only for the opening of menus.

    public bool turn_off_others; // this is for things like the perk menu so you only have 1 open at a time.
    public List<GameObject> others;
    
    public void toggle()
    {
        target.SetActive(!target.activeSelf);
        if (target.activeSelf && uncouple_mouse ) Cursor.lockState = CursorLockMode.Confined;
        if (!target.activeSelf && uncouple_mouse) Cursor.lockState = CursorLockMode.Locked;

        if (turn_off_others)
        {
            foreach (GameObject other in others)
            {
                other.SetActive(false);
            }
        }
    }
}
