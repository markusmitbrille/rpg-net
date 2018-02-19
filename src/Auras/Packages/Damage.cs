﻿public class Damage : Package
{
    public DamageType Type { get; set; }
    public float Amount { get; set; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public override bool IsValid => base.IsValid && Amount != 0f;

    public override Report Unwrap()
    {
        float old = Receiver.Life;
        Receiver.Life.Set(Receiver.Life - Amount);

        return new DamageReport(Source, Sender, Receiver, Type, old - Receiver.Life);
    }
}
