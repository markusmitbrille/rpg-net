using System;

public abstract class DamageEventArgs : EventArgs
{
    public Aura Source { get; }
    public Actor Sender { get; }

    protected DamageEventArgs(Aura source, Actor sender)
    {
        Source = source;
        Sender = sender;
    }
}