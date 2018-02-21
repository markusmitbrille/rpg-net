public class Attack : Package<bool>
{
    public Damage Damage { get; set; }

    public Attack(Skill origin, Aura source, Actor sender) : base(origin, source, sender)
    {
    }

    public override Report<bool> Unwrap()
    {
        if (Damage.IsNothing)
        {
            return ReportDamage();
        }
        else
        {
            return ReportNothing();
        }
    }

    private Report<bool> ReportDamage()
    {
        float old = Receiver.Life;
        Receiver.Life.Set(Receiver.Life - Damage.Amount);
        Damage = new Damage(Damage.Category, old - Receiver.Life);
        return new AttackReport(Origin, Source, Sender, Receiver, Damage.IsNothing, Damage);
    }

    private Report<bool> ReportNothing()
    {
        return new AttackReport(Origin, Source, Sender, Receiver, false, Damage);
    }
}