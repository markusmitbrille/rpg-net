using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class SimpleStat : Stat
{
    [SerializeField]
    [DataMember]
    private float value;

    public override float Value { get { return value; } }

    public SimpleStat(float value)
    {
        this.value = value;
    }
}