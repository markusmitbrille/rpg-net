using Autrage.LEX.NET.Serialization;
using System.Linq;

[DataContract]
public class StatSum : StatAggregate
{
    public override float Value => this.Sum(c => c);
}