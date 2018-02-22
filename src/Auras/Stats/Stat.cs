using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private StatInfo info;

    private Actor owner;

    public StatInfo Info => info;

    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());

    public abstract float Value { get; }

    public static implicit operator float(Stat stat) => stat.Value;
}