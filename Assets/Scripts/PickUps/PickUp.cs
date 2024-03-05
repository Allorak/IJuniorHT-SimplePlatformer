using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player) == false)
            return;

        ApplyEffect(player);
        Destroy(gameObject);
    }

    protected virtual void ApplyEffect(Player player)
    {
    }
}
