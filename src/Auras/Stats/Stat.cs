using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Stat : MonoBehaviour, IIdentifiable<StatInfo>
{
    [SerializeField]
    [DataMember]
    private StatInfo info;

    [DataMember]
    private Actor actor;

    public StatInfo Info => info;
    public Actor Actor => actor ?? (actor = GetComponentInParent<Actor>());

    public abstract float Value { get; }

    StatInfo IIdentifiable<StatInfo>.ID => Info;

    public static implicit operator float(Stat stat) => stat.Value;
}