using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class AimCentredSelector : ColliderSelector
{
    public override Vector3 Position => Owner.Aim;
    public override Quaternion Rotation => Quaternion.LookRotation(Owner.Aim - Owner.transform.position);
}
