using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    
    private float _currentHealth;
    
    public event Action Died;
    
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Applied damage can't be less than 0");

        _currentHealth -= damage;
        
        if(_currentHealth <= 0)
            Died?.Invoke();
    }
    
    public void Heal(float healAmount)
    {
        if (healAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(healAmount), "Heal amount can't be less than 0");

        _currentHealth += healAmount;
    }
}
