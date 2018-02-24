using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Info : Identity
{
    [SerializeField]
    [DataMember]
    private string title;

    [TextArea]
    [SerializeField]
    [DataMember]
    private string description;

    public string Title => title;
    public string Description => description;
}