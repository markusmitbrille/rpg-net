using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[DataContract]
public abstract class Effect : MonoBehaviour, IDestructible
{
    [SerializeField]
    [DataMember]
    private EffectInfo id;

    public EffectInfo ID => id;
    public Aura Aura { get; private set; }
    public Actor Owner { get; private set; }

    public void Destruct() => Destroy(this);

    private void Start()
    {
        Aura = GetComponentInParent<Aura>();
        if (Aura == null)
        {
            Bugger.Error($"Could not get {nameof(Aura)} of {GetType()} {this}!");
            Destruct();
            return;
        }

        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Bugger.Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destruct();
            return;
        }
    }
}