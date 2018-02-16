using Autrage.LEX.NET;
using System;

public class DamageEventArgs : EventArgs
{
    public Aura Source { get; }
    public Actor Origin { get; }

    public Actor OriginalTarget { get; }
    public DamageType OriginalType { get; }
    public float OriginalAmount { get; }

    public Actor Target { get; set; }
    public DamageType Type { get; set; }
    public float Amount { get; set; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;

    public DamageEventArgs(Aura source, Actor origin, Actor target, DamageType type, float amount)
    {
        origin.AssertNotNull(nameof(origin));
        target.AssertNotNull(nameof(target));

        Source = source;
        Origin = origin;

        OriginalTarget = target;
        OriginalType = type;
        OriginalAmount = amount;

        Target = target;
        Type = type;
        Amount = amount;
    }
}
