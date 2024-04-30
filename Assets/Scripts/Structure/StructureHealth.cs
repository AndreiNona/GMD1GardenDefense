using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StructureHealth : MonoBehaviour
{

    [SerializeField] [Tooltip("End of round reward value")]
    private int reward=2;
    
    [SerializeField] [Tooltip("Maximum health of the structure")]
    private int maxHealth = 5;
    
    private int _currentHealth;
    
    [SerializeField] [Tooltip("Grace period for additional hits")]
    private float timeInvincible = 2.0f;
    
    bool _isInvincible; 
    float _invincibleTimer;

    [SerializeField] private FloatingHealthBar _healthBar;
    
    void Start()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
        _healthBar.UpdateHealthBar(_currentHealth,maxHealth);
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
            {
                _isInvincible = false;
                _healthBar.ShowInvincible(_isInvincible);
            }
                
        }

        if (_currentHealth <= 0)
        {
            Debug.Log("Structure: "+gameObject.name + " has been destroyed");
            Destroy(gameObject);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (_isInvincible)
                return;

            _isInvincible = true;
            _invincibleTimer = timeInvincible;
        }
        
        _currentHealth +=  amount;
        _healthBar.UpdateHealthBar(_currentHealth,maxHealth);
        _healthBar.ShowInvincible(_isInvincible);
        Debug.Log("Structure: "+gameObject.name + " has" +_currentHealth +" / "+ maxHealth);

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Structure: "+gameObject.name + " has been hit");
            ChangeHealth(1);
        }
    }


}
