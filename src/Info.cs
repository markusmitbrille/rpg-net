using Autrage.LEX.NET.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Serialization/Info", order = -100)]
[DataContract]
public class Info : Identity
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