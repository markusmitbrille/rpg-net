public abstract class Report
{
    public Skill Origin { get; }
    public Aura Source { get; }
    public Actor Sender { get; }
    public Actor Receiver { get; }

    protected Report(Skill origin, Aura source, Actor sender, Actor receiver)
    {
        Source = source;
        Sender = sender;
        Receiver = receiver;
    }
}
