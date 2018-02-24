using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private StatInfo info;

    public StatInfo Info => info;
    public Parent<Actor> Owner { get; private set; }

    public abstract float Value { get; }

    public static implicit operator float(Stat stat) => stat.Value;

    private void Awake() => Owner = new Parent<Actor>(this);

    private void Start() => Owner.Fetch();
}