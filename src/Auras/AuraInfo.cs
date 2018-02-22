using Autrage.LEX.NET.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Serialization/Aura Info")]
[DataContract]
public class AuraInfo : Info
{
    [TextArea]
    [SerializeField]
    [DataMember]
    private string flavour;

    [Space]
    [BitMask(typeof(AuraTags))]
    [SerializeField]
    [DataMember]
    private AuraTags tags;

    [SerializeField]
    [DataMember]
    private AuraCategory category;

    public AuraTags Tags => tags;
    public AuraCategory Category => category;
    public string Flavour => flavour;
}