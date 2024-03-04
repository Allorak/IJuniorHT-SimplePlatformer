using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthPack : MonoBehaviour
{
    [SerializeField] private float _healAmount = 25;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player) == false)
            return;

        player.Health.Heal(_healAmount);
        Destroy(gameObject);
    }
}
