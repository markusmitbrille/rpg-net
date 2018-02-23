public abstract class Package<T> : IPackage
{
    private int ttl = 255;

    public Skill Origin { get; }
    public Aura Source { get; }
    public Actor Sender { get; }
    public Actor Receiver { get; set; }

    public virtual bool IsValid => Sender != null && Receiver != null && ttl > 0;

    protected Package(Skill origin, Aura source, Actor sender)
    {
        Origin = origin;
        Source = source;
        Sender = sender;
    }

    public abstract Report<T> Unwrap();

    public void Tick() => ttl--;

    IReport IPackage.Unwrap() => Unwrap();
}