using Autrage.LEX.NET.Serialization;
using System.Linq;

[DataContract]
public sealed class StatSum : StatAggregate
{
    public override float Value => this.Any() ? this.Sum(stat => stat) : 0f;
}