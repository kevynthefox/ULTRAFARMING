using UnityEngine;
using UnityEngine.InputSystem;

public class toggle_active : MonoBehaviour
{
    public GameObject target;

    public bool uncouple_mouse; //this is really only for the opening of menus.
    
    public void toggle()
    {
        target.SetActive(!target.activeSelf);
        if (target.activeSelf && uncouple_mouse ) Cursor.lockState = CursorLockMode.Confined;
        if (!target.activeSelf && uncouple_mouse) Cursor.lockState = CursorLockMode.Locked;
    }
}
