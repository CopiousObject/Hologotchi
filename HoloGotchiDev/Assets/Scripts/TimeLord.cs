using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLord : MonoBehaviour
{
    public float TimeScale = 1;

    private float OGFixedTimeStep;

    void Awake()
    {
        OGFixedTimeStep = Time.fixedDeltaTime;

        DontDestroyOnLoad(gameObject);
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
    }
}
