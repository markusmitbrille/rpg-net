public abstract class Report
{
    public Aura Source { get; }
    public Actor Sender { get; }
    public Actor Receiver { get; }

    protected Report(Aura source, Actor sender, Actor receiver)
    {
        Source = source;
        Sender = sender;
        Receiver = receiver;
    }
}
