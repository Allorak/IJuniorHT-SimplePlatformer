using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PickUp : MonoBehaviour
{
    [SerializeField] private AudioClip _collectSound;

    private void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    
    public virtual void ApplyEffect(Player player)
    {
        AudioSource.PlayClipAtPoint(_collectSound, transform.position);
        Destroy(gameObject);
    }
}
