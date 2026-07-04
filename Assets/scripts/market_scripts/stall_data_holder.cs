using System;
using UnityEngine;

public class stall_data_holder : MonoBehaviour
{
    public rhythm_controller controller_rhythm;
    public bool game_active;

    public void OnTriggerEnter(Collider other)
    {
        controller_rhythm.start_game();
    }
}
