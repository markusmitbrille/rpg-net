public interface IReport
{
    Skill Origin { get; }
    Aura Source { get; }
    Actor Sender { get; }
    Actor Receiver { get; }
    object Content { get; }
}
