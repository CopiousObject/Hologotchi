using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxToggle : MonoBehaviour
{
    [SerializeField] Sprite check;
    [SerializeField] Sprite x;

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
