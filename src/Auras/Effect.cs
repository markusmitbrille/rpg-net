﻿using Autrage.LEX.NET.Serialization;
using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Aura))]
[DataContract]
public abstract class Effect : MonoBehaviour
{
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

    /// <summary>
    /// Appends its own description to the longer description string and returns the <see cref="StringBuilder"/>;
    /// </summary>
    public abstract StringBuilder AppendDescription(StringBuilder description);

    [Flags]
    public enum StageResults
    {
        None = 0,
        Completed = 1 << 0,
        Terminated = 1 << 1,
        Failed = 1 << 2,
    }
}