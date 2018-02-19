using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class SpellEffect : Effect
{
    [SerializeField]
    [DataMember]
    private Aura prefab;

    public override string Description => $"Applies {prefab} to {Target} on completion.";

    public override void OnCompletion() => Owner.SendSpell(Aura.Origin, Aura, Target, prefab);
}
