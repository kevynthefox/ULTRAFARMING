using System;
using UnityEngine;
using UnityEngine.UI;


public class perk_button_logic : MonoBehaviour
{
    //public bool child_parent;
    public bool perk_weapon;

    public Image parent_image;
    public Image my_image;

    public int my_perk_num;
    public int slot_num;
    public Button my_button;
    
    public void Awake()
    {
        perk_logic.OnPerkSlotLogicEvent += update_state;
    }

    public void update_state(perk_logic perkLogic)
    {
        if (perkLogic.perk_slot_1_perk == my_perk_num || perkLogic.perk_slot_2_perk == my_perk_num ||
            perkLogic.perk_slot_3_perk == my_perk_num || perkLogic.perk_slot_4_perk == my_perk_num)
        {
            my_button.interactable = false;
        }
        else
        {
            my_button.interactable = true;
        }
    }

    public void perk_slot_logic_buttoned()
    {
        perk_logic.current.perk_slot_logic(my_perk_num,slot_num);
    }

    //public int 
}
