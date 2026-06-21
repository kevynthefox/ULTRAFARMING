using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class weapon_wheel_script : MonoBehaviour
{
    public int id;

    public string itemName;
    public TextMeshProUGUI itemText;
    //public Image selectedItem;
    private bool selected = false;
    //public Sprite icon;

    public weapon_wheel_controller controller;
    public AudioSource audio_source;
    public AudioClip audio_clip;
    
    void Update()
    {
        if (selected) //should probably change this to not use update later
        {
            //selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
    }
    public void Deselected()
    {
        selected = false;
    }

    public void HoverEnter()
    {
        itemText.text = itemName;
        controller.selected_weapon = id;
        audio_source.PlayOneShot(audio_clip);
    }
    
    public void HoverExit()
    {
        itemText.text = "";
    }

}
