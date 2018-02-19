﻿public class DamageReport : Report
{
    public DamageType Type { get; }
    public float Amount { get; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public DamageReport(Aura source, Actor sender, Actor receiver, DamageType type, float amount) : base(source, sender, receiver)
    {
        Type = type;
        Amount = amount;
    }
}