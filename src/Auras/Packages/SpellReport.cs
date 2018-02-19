public class SpellReport : Report
{
    public Aura Prefab { get; }

    public SpellReport(Skill origin, Aura source, Actor sender, Actor receiver, Aura prefab) : base(origin, source, sender, receiver)
    {
        Prefab = prefab;
    }
}
