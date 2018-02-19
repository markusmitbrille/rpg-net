using System;

public abstract class AuraEventArgs : EventArgs
{
    public Aura Source { get; }
    public Actor Sender { get; }

    protected AuraEventArgs(Aura source, Actor sender)
    {
        Source = source;
        Sender = sender;
    }
}