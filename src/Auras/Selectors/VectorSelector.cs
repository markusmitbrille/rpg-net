using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class VectorSelector : ColliderSelector
{
    [DataMember]
    private Vector3? shaft;

    public override Vector3 Position => shaft ?? Owner.Aim;
    public override Quaternion Rotation => Quaternion.LookRotation(Owner.Aim - shaft ?? Owner.transform.position);

    public override void ConfirmSelection()
    {
        if (shaft.HasValue)
        {
            base.ConfirmSelection();
        }
        else
        {
            shaft = Owner.Aim;
        }
    }
}