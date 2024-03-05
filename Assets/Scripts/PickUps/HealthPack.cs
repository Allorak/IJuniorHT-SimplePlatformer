using UnityEngine;

public class HealthPack : PickUp
{
    [SerializeField] private float _healAmount = 25;

    protected override void ApplyEffect(Player player)
    {
        player.Health.Heal(_healAmount);
    }
}
