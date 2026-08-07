using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weapon_wheel_controller : MonoBehaviour
{
    public static weapon_wheel_controller current;
    public GameObject wheel;
    public bool wheel_open;
    public FirstPersonLook look;
    public List<GameObject> weapons;

    public int selected_weapon;

    private void Awake()
    {
        current = this;
    }

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
            if (weapons.IndexOf(weapon) != selected_weapon)
            {
                if (weapon.TryGetComponent(out seed_bag_logic bag_logic))
                {
                    if (bag_logic.placed_forWeaponWheel == false)
                    {
                        //Debug.Log("bag was held,switching off");
                        weapon.SetActive(false);
                    } //this is so that the seed bag isn't turned off when placed on the ground and you switch weapons
                }
                else if (weapon.TryGetComponent(out harvester harvest_logic))
                {
                    if (harvest_logic.scythe_in_hand == true)
                    {
                        weapon.SetActive(false);
                    }
                }
                else if (weapon.TryGetComponent(out watering_can can_logic))
                {
                    if (can_logic.placed_forWeaponWheel == false)
                    {
                        //Debug.Log("bag was held,switching off");
                        weapon.SetActive(false);
                    } //this is so that the seed bag isn't turned off when placed on the ground and you switch weapons
                }
                else
                {
                    weapon.SetActive(false);   
                    
                }
            }
        }

        if (!weapons[selected_weapon].activeSelf)
        {
            weapons[selected_weapon].SetActive(true);
        }
    }
    
    
}
