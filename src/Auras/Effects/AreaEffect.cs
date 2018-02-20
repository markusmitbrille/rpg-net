using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class AreaEffect : Effect
{
    [SerializeField]
    [DataMember]
    private ActorSelector selector;

    public ActorSelector Selector => selector;
}
