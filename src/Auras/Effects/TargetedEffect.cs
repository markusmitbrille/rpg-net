using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using UnityEngine;

[DataContract]
public abstract class TargetedEffect : Effect
{
    [SerializeField]
    [DataMember]
    private Selector selector;

    [SerializeField]
    [DataMember]
    private bool aimOnUpdate;

    public IEnumerable<Actor> Targets => selector.Targets;
    public bool HasTargets => selector.HasTargets;
}