using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Player))]
public class PickUpCollector : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PickUp>(out var pickUp) == false)
            return;

        pickUp.ApplyEffect(_player);
    }
}
