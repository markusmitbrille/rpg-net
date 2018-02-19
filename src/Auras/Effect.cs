using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[RequireComponent(typeof(Aura))]
[DataContract]
public abstract class Effect : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    [DataMember]
    private int id;

    private Aura aura;
    private Actor owner;

    public abstract string Description { get; }

    public Aura Aura => aura ?? (aura = GetComponent<Aura>());
    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());
    public Actor Target => Owner?.Target;

    public virtual StageResults OnPreApplication() => StageResults.None;

    public virtual StageResults OnPreUpdate() => StageResults.None;

    public virtual StageResults OnApplication() => StageResults.None;

    public virtual StageResults OnUpdate() => StageResults.None;

    public virtual void OnCompletion()
    {
    }

    public virtual void OnTermination()
    {
    }

    public virtual void OnFailure()
    {
    }

    public virtual void OnConclusion()
    {
    }

    private void Update()
    {
        // Refresh owner once per tick
        owner = null;

        // Refresh aura once per tick
        aura = null;
    }

    [Flags]
    public enum StageResults
    {
        None = 0,
        Completed = 1 << 0,
        Terminated = 1 << 1,
        Failed = 1 << 2,
    }
}