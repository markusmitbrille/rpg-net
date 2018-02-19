public class PostDamageEventArgs : DamageEventArgs
{
    public Actor Receiver { get; }
    public DamageType Type { get; }
    public float Amount { get; }

    public bool IsDamage => Amount > 0f;
    public bool IsHealing => Amount < 0f;
    public bool IsNothing => Amount == 0f;

    public PostDamageEventArgs(Aura source, Actor sender, Actor receiver, DamageType type, float amount) : base(source, sender)
    {
        Receiver = receiver;
        Type = type;
        Amount = amount;
    }
}
