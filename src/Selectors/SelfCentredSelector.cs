using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class SelfCentredSelector : ColliderSelector
{
    public override Vector3 Position => Owner.transform.position;
    public override Quaternion Rotation => Owner.transform.rotation;
}
