using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public abstract class Effect : MonoBehaviour, IIdentifiable<EffectInfo>
{
    [SerializeField]
    [DataMember]
    private EffectInfo info;

    [DataMember]
    private Aura aura;

    [DataMember]
    private Actor actor;

    public EffectInfo Info => info;
    public Aura Aura => aura ?? (aura = GetComponent<Aura>());
    public Actor Actor => actor ?? (actor = GetComponentInParent<Actor>());

    EffectInfo IIdentifiable<EffectInfo>.ID => Info;

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
        // Self-destruct if no actor was found to avoid orphaned effects
        if (Actor == null)
        {
            Error($"Could not get actor of {this}!");
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