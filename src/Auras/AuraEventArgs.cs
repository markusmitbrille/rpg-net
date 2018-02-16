using Autrage.LEX.NET;
using System;

public class AuraEventArgs : EventArgs
{
    public Aura Source { get; }
    public Actor Origin { get; }

    public Actor OriginalTarget { get; }
    public Aura OriginalPrefab { get; }

    public Actor Target { get; set; }
    public Aura Prefab { get; set; }

    public AuraEventArgs(Aura source, Actor origin, Actor target, Aura prefab)
    {
        origin.AssertNotNull(nameof(origin));
        target.AssertNotNull(nameof(target));
        prefab.AssertNotNull(nameof(prefab));

        Source = source;
        Origin = origin;

        OriginalTarget = target;
        OriginalPrefab = prefab;

        Target = target;
        Prefab = prefab;
    }
}
