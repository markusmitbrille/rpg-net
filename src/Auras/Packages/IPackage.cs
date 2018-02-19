public interface IPackage
{
    Skill Origin { get; }
    Aura Source { get; }
    Actor Sender { get; }
    Actor Receiver { get; set; }

    bool IsValid { get; }

    IReport Unwrap();
    void Tick();
}