using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class window_test : MonoBehaviour
{
    [SerializeField] int displayLength = Display.displays.Length;
    [SerializeField] Display[] displays;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            displays[i] = Display.displays[i];
        }
        // Check the number of monitors connected.
        if (Display.displays.Length > 1)
        {
            // Activate the display 1 (second monitor connected to the system).
            Display.displays[1].Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
