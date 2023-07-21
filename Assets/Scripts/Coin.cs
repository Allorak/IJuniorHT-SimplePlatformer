using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _collectSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player player) == false)
            return;

        player.AddCoin();
        AudioSource.PlayClipAtPoint(_collectSound, transform.position);
        gameObject.SetActive(false);
    }
}
