using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[DataContract]
public abstract class Effect<T> : MonoBehaviour
    where T : EffectInfo
{
    [SerializeField]
    [DataMember]
    private T id;

    public T ID => id;
    public Aura Aura { get; private set; }
    public Actor Owner { get; private set; }

    private void Start()
    {
        Aura = GetComponentInParent<Aura>();
        if (Aura == null)
        {
            Bugger.Error($"Could not get {nameof(Aura)} of {GetType()} {this}!");
            Destroy(this);
            return;
        }

        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Bugger.Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destroy(this);
            return;
        }
    }
}