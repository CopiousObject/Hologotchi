using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    /// <summary>
    /// Quit the application when a button is pressed
    /// </summary>
    /// 
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
