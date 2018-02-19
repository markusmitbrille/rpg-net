using Autrage.LEX.NET.Serialization;

[DataContract]
public abstract class Stat
{
    public abstract float Value { get; }

    public static implicit operator float(Stat stat) => stat.Value;

    public static implicit operator Stat(float number) => new SimpleStat(number);
}