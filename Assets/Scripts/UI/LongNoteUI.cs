using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LongNoteUI : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void RefreshValue(float a)
    {
        slider.maxValue = a;
        slider.value = a;
    }

    public void SetValue(float a)
    {
        slider.value = a;
    }
}
