using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] [Tooltip("Maximum health of the enemy")]
    public int maxHealth = 5;
    private int _currentHealth;
    
    [SerializeField] [Tooltip("Grace period for additional hits")]
    public float timeInvincible = 0.5f;
    bool _isInvincible; 
    float _invincibleTimer;

    [SerializeField] private FloatingHealthBar _healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
        _healthBar.UpdateHealthBar(_currentHealth,maxHealth);
    }

    // Update is called once per frame
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
            Debug.Log("Enemy: "+gameObject.name + " has been saved");
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
        Debug.Log("Enemy: "+gameObject.name + " has" +_currentHealth +" / "+ maxHealth);


    }
    
}
