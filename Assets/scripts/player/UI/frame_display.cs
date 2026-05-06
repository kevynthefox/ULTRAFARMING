using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class frame_display : MonoBehaviour
{
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    public int frameRate;

    public TextMeshProUGUI frame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime)
        {
            frameRate = Mathf.RoundToInt(frameCount / time);

            frame.text = "fps: " + frameRate;

            time -= pollingTime;
            frameCount = 0;
        }
    
        
    }
}
