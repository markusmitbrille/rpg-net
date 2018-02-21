public class AttackReport : Report<bool>
{
    public Damage Damage { get; }

    public AttackReport(Skill origin, Aura source, Actor sender, Actor receiver, bool content, Damage damage) : base(origin, source, sender, receiver, content)
    {
        Damage = damage;
    }
}