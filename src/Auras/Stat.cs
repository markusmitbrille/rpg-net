using ProtoBuf;
using System;
using UnityEngine;

[ProtoContract(AsReferenceDefault = true)]
[ProtoInclude(1, typeof(SimpleStat))]
[ProtoInclude(2, typeof(ComplexStat))]
[ProtoInclude(3, typeof(Resource))]
public abstract class Stat
{
    public abstract float Value { get; }
}