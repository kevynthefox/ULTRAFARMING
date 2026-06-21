using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weapon_wheel_controller : MonoBehaviour
{
    public GameObject wheel;
    public bool wheel_open;
    public FirstPersonLook look;
    public List<GameObject> weapons;

    public int selected_weapon;
    
    public void toggle_wheel(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            wheel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0001f;
            look.enabled = false;
        }
        else
        {
            wheel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            look.enabled = true;
            
            equip_weapon();
        }
    }

    public void equip_weapon()
    {

        foreach (GameObject weapon in weapons)
        {
            if (weapons.IndexOf(weapon) != selected_weapon) weapon.SetActive(false);
        }

        if (weapons[selected_weapon] == null)
        {
            foreach (GameObject weapon in weapons)
            {
                if (weapons.IndexOf(weapon) != selected_weapon) weapon.SetActive(false);
            }
        }
        
        if (!weapons[selected_weapon].activeSelf)
        {
            weapons[selected_weapon].SetActive(true);
        }
    }
    
    
}
