using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Stat : MonoBehaviour, IUnique<StatInfo>, IExtendable<Stat>, IDestructible
{
    [SerializeField]
    [DataMember]
    private StatInfo id;

    public StatInfo ID => id;
    public Actor Owner { get; private set; }
    public abstract float Value { get; }

    public static implicit operator float(Stat stat) => stat.Value;

    public void Destruct() => Destroy(this);

    void IExtendable<Stat>.Extend(Stat other)
    {
    }

    protected virtual void Incorporate() => Owner.Stats.Add(this);

    protected virtual void Excorporate() => Owner.Stats.Remove(this);

    private void Start()
    {
        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Bugger.Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destruct();
            return;
        }

        Incorporate();
    }

    private void OnDestroy()
    {
        Excorporate();
    }
}