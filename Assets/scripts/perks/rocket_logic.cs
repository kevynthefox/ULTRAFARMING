using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket_logic : MonoBehaviour
{
    public Animator rocket_animator;

    public List<GameObject> objects_list;

    public AudioSource audio_source;
    
    public AudioClip launch_sound;
    public AudioClip land_sound;
    
    public IEnumerator arrive()
    {
        audio_source.PlayOneShot(land_sound);

        yield return new WaitForSeconds(5);

        transform.eulerAngles = Vector3.zero;
        rocket_animator.Play("door_open");
        
        foreach (GameObject obj in objects_list)
        {
            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
            obj.transform.parent = null;
        }
    }

    public void launch_from_button()
    {
        StartCoroutine(launch());
    }
    
    public IEnumerator launch()
    {
        rocket_animator.Play("door_close");

        foreach (GameObject obj in objects_list)
        {
            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            obj.transform.parent = this.transform;
        }
        
        audio_source.PlayOneShot(launch_sound);

        yield return new WaitForSeconds(5);
        transform.position = new Vector3(16, 29, 16);
        StartCoroutine(arrive());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null)
        {
            if (!objects_list.Contains(other.gameObject))
            {
                objects_list.Add(other.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null)
        {
            if (objects_list.Contains(other.gameObject))
            {
                objects_list.Remove(other.gameObject);
            }
        }
    }
}
