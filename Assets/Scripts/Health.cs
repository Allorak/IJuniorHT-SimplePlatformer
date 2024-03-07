using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> HealthChanged;
    
    [SerializeField] private float _health;

    public void ApplyDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Applied damage can't be less than 0");

        _health -= damage;
        
        HealthChanged?.Invoke(_health);
    }
    
    public void Heal(float healAmount)
    {
        if (healAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(healAmount), "Heal amount can't be less than 0");

        _health += healAmount;
        
        HealthChanged?.Invoke(_health);
    }
}
