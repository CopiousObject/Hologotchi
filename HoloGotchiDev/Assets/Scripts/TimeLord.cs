using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLord : MonoBehaviour
{
    [Range(0, 100)]
    public float TimeScale = 1;
    public float TimeIncrement = 1;

    [SerializeField]
    private float TotalTime;
    [SerializeField]
    private float DeltaTime;

    private float OGFixedTimeStep;

    void Awake()
    {
        OGFixedTimeStep = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TimeScale += TimeIncrement;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TimeScale -= TimeIncrement;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TimeScale = 1f;
        }
        if (TimeScale < 0)
            {
                TimeScale = 0;
            }

        Time.timeScale = TimeScale;
        //Time.fixedDeltaTime = OGFixedTimeStep / TimeScale; // absolutely destroys framerate at higher speeds

        TotalTime = Time.time;
        DeltaTime = Time.deltaTime;
    }
}
