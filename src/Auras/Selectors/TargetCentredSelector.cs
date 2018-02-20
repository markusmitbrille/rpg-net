using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class TargetCentredSelector : ColliderSelector
{
    public override Vector3 Position => Owner.Target.transform.position;
    public override Quaternion Rotation => Owner.Target.transform.rotation;
}
