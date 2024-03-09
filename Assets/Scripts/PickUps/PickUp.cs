using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PickUp : MonoBehaviour
{
    [SerializeField] private AudioClip _collectSound;

    private void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player) == false)
            return;

        ApplyEffect(player);
        AudioSource.PlayClipAtPoint(_collectSound, transform.position);
        Destroy(gameObject);
    }

    protected abstract void ApplyEffect(Player player);
}
