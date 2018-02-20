using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;

[DataContract]
public sealed class OwnerSelector : Selector
{
    public override IEnumerable<Actor> Targets => new Actor[1] { Owner };

    public override bool HasTargets => true;
}