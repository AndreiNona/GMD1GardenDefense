using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Color defaultColor = Color.green; // Color when not invincible
    [SerializeField] private Color invincibleColor = Color.yellow; // Color when invincible

    private Image fillImage; // This will hold the reference to the slider's fill component

    void Awake()
    {
        // Get the Image component of the slider's fill area
        fillImage = slider.fillRect.GetComponent<Image>();
        fillImage.color = defaultColor; // Set the initial color to default
    }

    public void UpdateHealthBar(int currentValue, int maxValue)
    {
        slider.value = (float)currentValue / (float)maxValue;
    }

    public void ShowInvincible(bool isInvincible)
    {
        // Change the color based on the invincibility status
        fillImage.color = isInvincible ? invincibleColor : defaultColor;
    }
}
