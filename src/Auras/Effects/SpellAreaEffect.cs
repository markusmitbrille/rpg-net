using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DataContract]
public class SpellAreaEffect : AreaEffect
{
    [SerializeField]
    [DataMember]
    private Aura prefab;

    [DataMember]
    private List<Aura> children = new List<Aura>();

    public override string Description => $"Applies {prefab} to {Selector} on completion.";

    public override void OnCompletion()
    {
        foreach (Actor target in Selector)
        {
            children.Add(Owner.SendSpell(Aura.Origin, Aura, target, prefab));
        }
    }

    public override bool CanDestroy() => children.All(child => child == null);
}