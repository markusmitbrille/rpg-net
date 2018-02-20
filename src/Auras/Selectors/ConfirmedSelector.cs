using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class ConfirmedSelector : Selector, IConfirmSelectionTarget
{
    [SerializeField]
    [DataMember]
    private bool needsConfirmation = true;

    [DataMember]
    private bool hasConfirmed;

    public override bool HasTargets => needsConfirmation ? hasConfirmed : true;

    public virtual void ConfirmSelection() => hasConfirmed = true;
}
