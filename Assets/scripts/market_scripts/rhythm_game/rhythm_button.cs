using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class rhythm_button : MonoBehaviour
{
    public rhythm_controller rhythm_controller;
    public List<GameObject> objects_in_bounds;

    public void rhythm_press(InputAction.CallbackContext context)
    {

        if (rhythm_controller.is_game_running == true)
        {
            if (context.started)
            {
                if (objects_in_bounds.Count > 0)
                {
                    rhythm_controller.score +=
                        10 * (1 / Vector3.Distance(objects_in_bounds[0].transform.position, this.transform.position));
                    objects_in_bounds.RemoveAt(0);
                }
                else
                {
                    rhythm_controller.score -= 10;
                }

                rhythm_controller.score_text.text = "Score: " + rhythm_controller.score;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rhythm_game_beat"))
        {
            objects_in_bounds.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("rhythm_game_beat"))
        {
            rhythm_controller.score -= 10;
            rhythm_controller.score_text.text = "Score: " + rhythm_controller.score;
            objects_in_bounds.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
