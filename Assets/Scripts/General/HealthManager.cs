using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] [Tooltip("Maximum health of the object")]
    private int maxHealth = 5;
    private int _currentHealth;

    [Header("Invincibility Settings")]
    [SerializeField] [Tooltip("Grace period for additional hits")]
    private float timeInvincible = 1.0f;
    private bool _isInvincible;
    private float _invincibleTimer;

    [Header("Health Bar Display")]
    [SerializeField] private FloatingHealthBar _healthBar;

    [Header("Behaviour on Health Depletion")]
    [SerializeField] private bool destroyOnDepletion = true;
    [SerializeField] private string depletionMessage = "Object has been destroyed";

    void Start()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0)
            {
                _isInvincible = false;
                _healthBar.ShowInvincible(false);
            }
        }

        if (_currentHealth <= 0)
        {
            Debug.Log($"{depletionMessage}: {gameObject.name}");
            if (destroyOnDepletion)
                Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        amount = -amount;
        if (_isInvincible && amount < 0)
            return;

        if (amount < 0)
        {
            _isInvincible = true;
            _invincibleTimer = timeInvincible;
        }

        _currentHealth += amount;
        if (_healthBar)
        {
            _healthBar.UpdateHealthBar(_currentHealth, maxHealth);
            _healthBar.ShowInvincible(_isInvincible);
        }
        Debug.Log($"{gameObject.name} has {_currentHealth} / {maxHealth}");
    }
}
