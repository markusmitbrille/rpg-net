public class Spell : Package<Aura>
{
    public Aura Prefab { get; set; }

    public override bool IsValid => base.IsValid && Prefab != null;

    public Spell(Skill origin, Aura source, Actor sender) : base(origin, source, sender)
    {
    }

    public override Report<Aura> Unwrap()
    {
        Aura content = Prefab.Instantiate(Receiver, Origin, Source);
        return new SpellReport(Origin, Source, Sender, Receiver, content, Prefab);
    }
}