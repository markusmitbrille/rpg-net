using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DataContract]
public class SpellAreaEffect : TargetedEffect
{
    [SerializeField]
    [DataMember]
    private Aura prefab;

    [DataMember]
    private List<Aura> children = new List<Aura>();
}