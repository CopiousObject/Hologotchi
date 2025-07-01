using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxToggle : MonoBehaviour
{
    [SerializeField] Sprite check;
    [SerializeField] Sprite x;

    /// <summary>
    /// Swaps which checkbox asset is displayed for later
    /// use in what notifications the player gets
    /// </summary>
    public void Toggle()
    {
        if (this.GetComponent<Image>().sprite == check)
        {
            this.GetComponent<Image>().sprite = x;
        }
        else
        {
            this.GetComponent<Image>().sprite = check;
        }
    }
}
