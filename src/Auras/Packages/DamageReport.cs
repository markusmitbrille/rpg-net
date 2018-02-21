public class DamageReport : Report<bool>
{
    public DamageCategory Category { get; }
    public float Amount { get; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public DamageReport(Skill origin, Aura source, Actor sender, Actor receiver, bool content, DamageCategory category, float amount) : base(origin, source, sender, receiver, content)
    {
        Category = category;
        Amount = amount;
    }
}