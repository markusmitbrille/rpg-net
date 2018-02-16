using ProtoBuf;
using System;
using UnityEngine;

/// <summary>
/// Encapsulates a singular <see cref="float"/> value.
/// </summary>
[Serializable]
[ProtoContract(AsReferenceDefault = true)]
public class SimpleStat : Stat
{
    [SerializeField]
    [ProtoMember(1)]
    private float value = 0f;
    public override float Value { get { return value; } }

    public SimpleStat(float value)
    {
        this.value = value;
    }
}
