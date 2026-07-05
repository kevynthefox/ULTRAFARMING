using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class rhythm_controller : MonoBehaviour
{
    public float score;

    public bool is_game_running;

    public int number_of_beats;
    public int base_number_of_beats;

    public float random_time;
    public float max_random_time;

    public List<Transform> spawn_points;
    public GameObject beat_prefab;
    
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI beats_text;

    public sell seller;

    public GameObject assigned_shopkeeper;
    public bool auto_play;
    public Transform lock_point;
    public stall_data_holder holder;

    public void start_game()
    {
        number_of_beats = base_number_of_beats;
        score = 0;
        beats_text.text = "beats: " + number_of_beats;
        score_text.text = "score:" +  score;
        
        assigned_shopkeeper.transform.position = lock_point.position;
        if (assigned_shopkeeper.name == "Player")
        {
            assigned_shopkeeper.GetComponent<FirstPersonMovement>().enabled = false;
        }

        if (is_game_running == false) StartCoroutine(spawn_beats());
    }
    
    public IEnumerator spawn_beats()
    {
        is_game_running = true;
        holder.game_active = true;
        while (is_game_running == true)
        {
            if (number_of_beats > 0)
            {
                Instantiate(beat_prefab, spawn_points[Random.Range(0, spawn_points.Count)].position,
                    quaternion.identity);

                number_of_beats--;
                beats_text.text = "beats: " + number_of_beats.ToString();
                random_time = UnityEngine.Random.Range(0, max_random_time);
                yield return new WaitForSeconds(0.1f + random_time);
            }
            else
            {
                is_game_running = false;
                holder.game_active = false;
                if (assigned_shopkeeper.name == "Player")
                {
                    assigned_shopkeeper.GetComponent<FirstPersonMovement>().enabled = true;
                }
                seller.sell_product();
                yield break;
            }
        }
    }
}
