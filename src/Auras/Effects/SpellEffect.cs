using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DataContract]
public class SpellEffect : Effect
{
    [SerializeField]
    [DataMember]
    private Aura prefab;

    [SerializeField]
    [DataMember]
    private ActorSelector selector;

    [DataMember]
    private List<Aura> children = new List<Aura>();

    public override string Description => $"Applies {prefab} to {selector} on completion.";

    public override void OnCompletion()
    {
        foreach (Actor target in selector)
        {
            children.Add(Owner.SendSpell(Aura.Origin, Aura, target, prefab));
        }
    }

    public override bool CanDestroy() => children.All(child => child == null);
}