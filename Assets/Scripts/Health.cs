using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    
    private float _currentHealth;
    
    public event Action Died;
    public event Action<float, float> HealthChanged;
    
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Applied damage can't be less than 0");

        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);
       
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
             
        if(_currentHealth == 0)
            Died?.Invoke();
    }
    
    public void Heal(float healAmount)
    {
        if (healAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(healAmount), "Heal amount can't be less than 0");

        _currentHealth += healAmount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
        
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
