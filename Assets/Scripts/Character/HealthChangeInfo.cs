
public struct HealthChangeInfo
{
    public Health health { get; private set; }
    public int Amount { get; private set; }

    public HealthChangeInfo(Health health, int changeAmount)
    {
        this.health = health;
        this.Amount = changeAmount;
    }
}
