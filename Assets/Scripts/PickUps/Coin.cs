public class Coin : PickUp
{
    public override void ApplyEffect(Player player)
    {
        player.AddCoin();
        base.ApplyEffect(player);
    }
}
