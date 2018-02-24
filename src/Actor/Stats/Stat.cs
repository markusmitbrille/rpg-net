using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private StatInfo info;

    public StatInfo Info => info;
    public Actor Owner { get; private set; }
    public abstract float Value { get; }

    public static implicit operator float(Stat stat) => stat.Value;

    private void Start()
    {
        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Bugger.Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destroy(this);
            return;
        }
    }
}