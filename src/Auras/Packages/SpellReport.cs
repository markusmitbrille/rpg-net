public class SpellReport : Report
{
    public Aura Prefab { get; }

    public SpellReport(Aura source, Actor sender, Actor receiver, Aura prefab) : base(source, sender, receiver)
    {
        Prefab = prefab;
    }
}
