public abstract class Package
{
    private int ttl = 255;

    public Aura Source { get; set; }
    public Actor Sender { get; set; }
    public Actor Receiver { get; set; }

    public virtual bool IsValid => Source != null && Sender != null && Receiver != null && ttl > 0;

    public abstract Report Unwrap();

    public void Tick() => ttl--;
}
