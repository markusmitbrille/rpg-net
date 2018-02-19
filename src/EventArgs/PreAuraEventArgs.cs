public class PreAuraEventArgs : AuraEventArgs
{
    public Actor OriginalReceiver { get; }
    public Aura OriginalPrefab { get; }

    public Actor Receiver { get; set; }
    public Aura Prefab { get; set; }

    public bool IsInvalid => Receiver == null || Prefab == null;

    public PreAuraEventArgs(Aura source, Actor sender, Actor receiver, Aura prefab) : base(source, sender)
    {
        OriginalReceiver = receiver;
        OriginalPrefab = prefab;

        Receiver = receiver;
        Prefab = prefab;
    }
}
