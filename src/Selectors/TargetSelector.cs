using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;

[DataContract]
public sealed class TargetSelector : Selector
{
    public override IEnumerable<Actor> Targets => new Actor[1] { Owner.Target };

    public override bool HasTargets => Owner.Target != null;
}