public class PostAuraEventArgs : AuraEventArgs
{
    public Actor Receiver { get; }
    public Aura Prefab { get; }

    public PostAuraEventArgs(Aura source, Actor sender, Actor receiver, Aura prefab) : base(source, sender)
    {
        Receiver = receiver;
        Prefab = prefab;
    }
}
