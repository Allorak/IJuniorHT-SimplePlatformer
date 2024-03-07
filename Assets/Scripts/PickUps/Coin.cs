public class Coin : PickUp
{
    protected override void ApplyEffect(Player player)
    {
        player.AddCoin();
    }
}
