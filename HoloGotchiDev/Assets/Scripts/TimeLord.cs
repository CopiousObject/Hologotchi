using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLord : MonoBehaviour
{
    [Range(0, 100)]
    public float TimeScale = 1;

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
            TimeScale += 0.25f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TimeScale -= 0.25f;
        }
        if (TimeScale < 0)
        {
            TimeScale = 0;
        }

        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = OGFixedTimeStep * TimeScale;

        TotalTime = Time.time;
        DeltaTime = Time.deltaTime;
    }
}
