using UnityEngine;

public class Coin : PickUp
{
    [SerializeField] private AudioClip _collectSound;

    protected override void ApplyEffect(Player player)
    {
        player.AddCoin();
        AudioSource.PlayClipAtPoint(_collectSound, transform.position);
    }
}
