public class DamageReport : Report<bool>
{
    public DamageType Type { get; }
    public float Amount { get; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public DamageReport(Skill origin, Aura source, Actor sender, Actor receiver, bool content, DamageType type, float amount) : base(origin, source, sender, receiver, content)
    {
        Type = type;
        Amount = amount;
    }
}