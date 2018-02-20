using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class SpellEffect : Effect
{
    [SerializeField]
    [DataMember]
    private Aura prefab;

    [DataMember]
    private Aura child;

    public override string Description => $"Applies {prefab} to {Target} on completion.";

    public override void OnCompletion() => child = Owner.SendSpell(Aura.Origin, Aura, Target, prefab);

    public override bool CanDestroy() => child == null;
}