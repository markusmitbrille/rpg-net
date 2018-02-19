public class PreDamageEventArgs : DamageEventArgs
{
    public Actor OriginalReceiver { get; }
    public DamageType OriginalType { get; }
    public float OriginalAmount { get; }

    public Actor Receiver { get; set; }
    public DamageType Type { get; set; }
    public float Amount { get; set; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;
    public bool IsNothing => Amount == 0f;

    public bool IsInvalid => Receiver == null || IsNothing;

    public PreDamageEventArgs(Aura source, Actor sender, Actor receiver, DamageType type, float amount) : base(source, sender)
    {
        OriginalReceiver = receiver;
        OriginalType = type;
        OriginalAmount = amount;

        Receiver = receiver;
        Type = type;
        Amount = amount;
    }
}
