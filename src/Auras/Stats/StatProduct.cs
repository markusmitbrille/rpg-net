using Autrage.LEX.NET.Serialization;
using System.Linq;

[DataContract]
public class StatProduct : StatAggregate
{
    public override float Value => this.Aggregate(1f, (acc, src) => acc * src);
}
