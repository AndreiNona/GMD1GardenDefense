using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathCollectable : MonoBehaviour
{
    [SerializeField] [Tooltip("How much the item heals")]
    private int healPower = 2;
    public AudioClip collectedClip;
    
    public void SetHealPower(int baseValue, int coefficient)
    {
        healPower = baseValue * coefficient;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            if(controller.health  < controller.maxHealth)
            {
                controller.ChangeHealth(healPower);
                Destroy(gameObject);
                
                controller.PlaySound(collectedClip);
            }
        }
    }
}
