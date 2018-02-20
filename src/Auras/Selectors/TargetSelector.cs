using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using UnityEngine;

[DataContract]
public sealed class TargetSelector : ConfirmedSelector
{
    public override Vector3 Position => Owner.Target?.transform.position ?? Owner.transform.position;
    public override Quaternion Rotation => Owner.Target?.transform.rotation ?? Owner.transform.rotation;

    public override IEnumerable<Actor> Targets => new Actor[1] { Owner.Target };
    public override bool HasTargets => base.HasTargets && Owner.Target != null;
}