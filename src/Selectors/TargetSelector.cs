using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public sealed class TargetSelector : ActorSelector
{
    private Actor owner;

    public override IEnumerator<Actor> GetEnumerator()
    {
        yield return owner.Target;
    }

    private void Start()
    {
        owner = GetComponentInParent<Actor>();
        if (owner == null)
        {
            Warning($"Could not find owner of {this}!");
            Destroy(this);
        }
    }
}