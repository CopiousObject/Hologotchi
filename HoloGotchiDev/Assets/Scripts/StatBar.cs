using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    void Awake()
    {
        slider.onValueChanged.AddListener(ColorChange);
    }

    void ColorChange(float value)
    {
        // switch to red
        if (value <= .33f)
        {
            fill.color = new Color(0.666f, 0.498f, 0.407f);
        }
        // switch to yellow
        else if (value <= .66f)
        {
            fill.color = new Color(0.666f, 0.666f, 0.407f);
        }
        // stay/return to green
        else
        {
            fill.color = new Color(0.498f, 0.666f, 0.407f);
        }
    }
}
