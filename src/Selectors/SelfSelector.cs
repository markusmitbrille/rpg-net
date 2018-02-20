using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using UnityEngine;

[DataContract]
public sealed class SelfSelector : ConfirmedSelector
{
    public override Vector3 Position => Owner.transform.position;
    public override Quaternion Rotation => Owner.transform.rotation;

    public override IEnumerable<Actor> Targets => new Actor[1] { Owner };
}