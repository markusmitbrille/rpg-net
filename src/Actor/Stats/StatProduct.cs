using Autrage.LEX.NET.Serialization;
using System.Linq;

[DataContract]
public sealed class StatProduct : StatAggregate
{
    public override float Value => this.Any() ? this.Aggregate(1f, (product, stat) => product * stat) : 1f;
}