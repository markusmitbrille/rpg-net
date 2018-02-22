using Autrage.LEX.NET.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Serialization/Skill Info")]
[DataContract]
public class SkillInfo : Info
{
    [SerializeField]
    [DataMember]
    private Aura active;

    [SerializeField]
    [DataMember]
    private Aura passive;

    public Aura Active => active;
    public Aura Passive => passive;
}