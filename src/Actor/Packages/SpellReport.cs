public class SpellReport : Report<Aura>
{
    public Aura Prefab { get; }

    public SpellReport(Skill origin, Aura source, Actor sender, Actor receiver, Aura content, Aura prefab) : base(origin, source, sender, receiver, content)
    {
        Prefab = prefab;
    }
}
