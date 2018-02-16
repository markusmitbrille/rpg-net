using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Aura))]
public abstract class Effect : DataDrivenBehaviour
{
    [Flags]
    public enum StageResults
    {
        None = 0,
        Completed = 1 << 0,
        Terminated = 1 << 1,
        Failed = 1 << 2,
    }

    public virtual StageResults OnPreApplication() { return StageResults.None; }
    public virtual StageResults OnPreUpdate() { return StageResults.None; }

    public virtual StageResults OnApplication() { return StageResults.None; }
    public virtual StageResults OnUpdate() { return StageResults.None; }

    public virtual void OnCompletion() { }
    public virtual void OnTermination() { }
    public virtual void OnFailure() { }

    public virtual void OnConclusion() { }

    /// <summary>
    /// Appends its own description to the longer description string and returns the <see cref="StringBuilder"/>;
    /// </summary>
    public abstract StringBuilder AppendDescription(StringBuilder description);
}
