using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] Image _iconFull;
    [SerializeField] Image _iconEmpty;
    [SerializeField] Slider slider;

    public float SliderValue { get => slider.value; set => slider.value = value; }
}
