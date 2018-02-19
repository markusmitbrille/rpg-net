public class Damage : Package<bool>
{
    public DamageType Type { get; set; }
    public float Amount { get; set; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public override bool IsValid => base.IsValid && Amount != 0f;

    public Damage(Skill origin, Aura source, Actor sender) : base(origin, source, sender)
    {
    }

    public override Report<bool> Unwrap()
    {
        float old = Receiver.Life;
        Receiver.Life.Set(Receiver.Life - Amount);

        float actual = old - Receiver.Life;
        return new DamageReport(Origin, Source, Sender, Receiver, actual != 0f, Type, actual);
    }
}