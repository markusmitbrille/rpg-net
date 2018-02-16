using Autrage.LEX.NET.Serialization;
using UnityEngine;

/// <summary>
/// Encapsulates a singular <see cref="float"/> value.
/// </summary>
[DataContract]
public class SimpleStat : Stat
{
    [SerializeField]
    [DataMember]
    private float value = 0f;

    public override float Value { get { return value; } }

    public SimpleStat(float value)
    {
        this.value = value;
    }
}