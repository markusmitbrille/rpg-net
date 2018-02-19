using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class SpellEffect : Effect
{
    [SerializeField]
    [DataMember]
    private Aura Prefab;

    public override string Description => $"Applies {Prefab} to {Target} on completion.";

    public override void OnCompletion() => Owner.SendSpell(Aura, Target, Prefab);
}
