using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

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

    public virtual StageResults OnPreApplication() => StageResults.None;

    public virtual StageResults OnPreUpdate() => StageResults.None;

    public virtual StageResults OnApplication() => StageResults.CanComplete;

    public virtual StageResults OnUpdate() => StageResults.CanComplete;

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

    public virtual bool CanDestroy() => true;

    private void Start()
    {
        // Self-destruct if no owner was found to avoid orphaned effects
        if (Owner == null)
        {
            Error($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }

        // Self-destruct if no aura was found to avoid orphaned effects
        if (Aura == null)
        {
            Error($"Could not get aura of {this}!");
            Destroy(this);
            return;
        }
    }

    [Flags]
    public enum StageResults
    {
        None = 0,
        CanComplete = 1 << 0,
        Terminated = 1 << 1,
        Failed = 1 << 2,
    }
}