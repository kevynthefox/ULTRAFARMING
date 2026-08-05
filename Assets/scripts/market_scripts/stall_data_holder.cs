using System;
using UnityEngine;

public class stall_data_holder : MonoBehaviour
{
    public rhythm_controller controller_rhythm;
    public bool game_active;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out nav_pathfinding pathfinding))
        {
            if (pathfinding.destinations[0] == this.transform)
            {
                controller_rhythm.start_game();
            }
        }
    }
}
