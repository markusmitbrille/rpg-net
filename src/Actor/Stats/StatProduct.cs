using Autrage.LEX.NET.Serialization;
using System.Linq;

[DataContract]
public class StatProduct : StatAggregate
{
    public override float Value => this.Aggregate(1f, (product, stat) => product * stat);
}
