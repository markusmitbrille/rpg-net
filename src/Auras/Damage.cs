public struct Damage
{
    public DamageCategory Category { get; }
    public float Amount { get; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;
    public bool IsNothing => Amount == 0f;

    public Damage(DamageCategory category, float amount) : this()
    {
        Category = category;
        Amount = amount;
    }
}
