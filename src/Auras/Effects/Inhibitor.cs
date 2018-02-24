public abstract class Inhibitor : Effect
{
    public abstract bool AllowStart { get; }
    public abstract bool AllowUpdate { get; }
    public abstract bool AllowCompletion { get; }

    private void Start() => Aura.Instance?.Inhibitors.Fetch();

    private void OnDestroy() => Aura.Instance?.Inhibitors.Fetch();
}
