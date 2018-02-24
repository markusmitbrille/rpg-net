public abstract class Conductor : Effect
{
    public abstract void ConductStart();

    public abstract void ConductUpdate();

    public abstract void ConductCompletion();

    public abstract void ConductTermination();

    public abstract void ConductAbortion();

    public abstract void ConductConclusion();

    private void Start() => Aura.Instance?.Conductors.Fetch();

    private void OnDestroy() => Aura.Instance?.Conductors.Fetch();
}