public abstract class Report<T> : IReport
{
    public Skill Origin { get; }
    public Aura Source { get; }
    public Actor Sender { get; }
    public Actor Receiver { get; }
    public T Content { get; }

    object IReport.Content => Content;

    protected Report(Skill origin, Aura source, Actor sender, Actor receiver, T content)
    {
        Source = source;
        Sender = sender;
        Receiver = receiver;
        Content = content;
    }
}