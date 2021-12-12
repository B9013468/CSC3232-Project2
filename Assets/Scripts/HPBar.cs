using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;

    public void MaxHealth(int hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void AdjustHealth(int hp)
    {
        slider.value = hp;
    }
}
