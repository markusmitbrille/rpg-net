using Autrage.LEX.NET.Extensions;
using Autrage.LEX.NET.Serialization;
using System.Collections;
using System.Collections.Generic;

[DataContract]
public abstract class StatAggregate : Stat, IEnumerable<Stat>
{
    [DataMember]
    private List<Stat> contributors;

    public void Add(Stat stat) => contributors.Add(stat);

    public void Add(params Stat[] stats) => contributors.AddRange(stats);

    public void Remove(params Stat[] stats) => stats.ForEach(stat => contributors.Remove(stat));

    IEnumerator<Stat> IEnumerable<Stat>.GetEnumerator() => contributors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => contributors.GetEnumerator();
}