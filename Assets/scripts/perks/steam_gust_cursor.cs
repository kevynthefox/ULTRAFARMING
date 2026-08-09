using System;
using System.Collections;
using UnityEngine;

public class steam_gust_cursor : MonoBehaviour
{
    public bool activate;
    public float force;
    
    public AudioSource audio_source;
    public AudioClip audio_clip;
    

    public IEnumerator pow(float force_)
    {
    
        force = force_;
        activate = true;

        //audio_source.volume = force/10;
        audio_source.PlayOneShot(audio_clip);

        yield return new WaitForSeconds(0.2f);
        
        activate = false;
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (activate)
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(force,transform.position,force);
            }
        }
    }
}
