using ProtoBuf;
using System;
using UnityEngine;

/// <summary>
/// A special composite stat that represents a value bounded by <see cref="ComplexStat"/>s, which
/// regenerates by <see cref="regen"/> over time and can be reset to a <see cref="def"/>.
/// </summary>
[Serializable]
[ProtoContract(AsReferenceDefault = true)]
public class Resource : Stat
{
    [SerializeField]
    [ProtoMember(1)]
    private float value = 0f;
    public override float Value
    {
        get
        {
            return value;
        }
    }

    /// <summary>
    /// The default value to which the resource can be <see cref="Reset"/>.
    /// </summary>
    [SerializeField]
    [ProtoMember(2)]
    private ComplexStat def = new ComplexStat();
    public ComplexStat Def { get { return def; } }

    /// <summary>
    /// Minimum value used in <see cref="Reset"/> and <see cref="Regenerate"/>. Only effective
    /// when <see cref="max"/> is set and greater than or equal to this.
    /// </summary>
    [SerializeField]
    [ProtoMember(3)]
    private ComplexStat min = new ComplexStat();
    public ComplexStat Min { get { return min; } }

    /// <summary>
    /// Maximum value used in <see cref="Reset"/> and <see cref="Regenerate"/>. Only effective
    /// when <see cref="min"/> is set and less than or equal to this.
    /// </summary>
    [SerializeField]
    [ProtoMember(4)]
    private ComplexStat max = new ComplexStat();
    public ComplexStat Max { get { return max; } }

    /// <summary>
    /// Regeneration rate used in <see cref="Regenerate"/>.
    /// </summary>
    [SerializeField]
    [ProtoMember(5)]
    private ComplexStat regen = new ComplexStat();
    public ComplexStat Regen { get { return regen; } }

    /// <summary>
    /// Resets the <see cref="value"/> to its default, if there is one, clamped 
    /// by <see cref="min"/> and <see cref="max"/>, if they are valid.
    /// </summary>
    public void Reset()
    {
        if (def == null)
        {
            return;
        }

        if (min == null && max == null || min?.Value > max?.Value)
        {
            // No bounds set or invalid
            value = def.Value;
        }
        else
        {
            if (min != null)
            {
                value = Mathf.Max(min.Value, def.Value);
            }
            if (max != null)
            {
                value = Mathf.Min(min.Value, def.Value);
            }
        }
    }

    /// <summary>
    /// Increases <see cref="value"/> by <see cref="regen"/> times <see cref="Time.deltaTime"/>, 
    /// if there is one, clamped by <see cref="min"/> and <see cref="max"/>, if they are valid.
    /// </summary>
    public void Regenerate()
    {
        if (regen == null)
        {
            return;
        }

        float result = value + regen.Value * Time.deltaTime;

        if (min == null && max == null || min?.Value > max?.Value)
        {
            // No bounds set or invalid
            value = result;
        }
        else
        {
            if (min != null)
            {
                value = Mathf.Max(min.Value, result);
            }
            if (max != null)
            {
                value = Mathf.Min(min.Value, result);
            }
        }
    }

    public void SetValue(float value)
    {
        if (min == null && max == null || min?.Value > max?.Value)
        {
            // No bounds set or invalid
            this.value = value;
        }
        else
        {
            if (min != null)
            {
                this.value = Mathf.Max(min.Value, value);
            }
            if (max != null)
            {
                this.value = Mathf.Min(min.Value, value);
            }
        }
    }

    public static Resource operator +(Resource resource, float number)
    {
        resource.SetValue(resource.value + number);
        return resource;
    }

    public static Resource operator +(float number, Resource resource)
    {
        resource.SetValue(resource.value + number);
        return resource;
    }

    public static Resource operator -(Resource resource, float number)
    {
        resource.SetValue(resource.value - number);
        return resource;
    }

    public static Resource operator -(float number, Resource resource)
    {
        resource.SetValue(number - resource.value);
        return resource;
    }

    public static Resource operator *(Resource resource, float number)
    {
        resource.SetValue(resource.value * number);
        return resource;
    }

    public static Resource operator *(float number, Resource resource)
    {
        resource.SetValue(resource.value * number);
        return resource;
    }

    public static Resource operator /(Resource resource, float number)
    {
        resource.SetValue(resource.value / number);
        return resource;
    }

    public static Resource operator /(float number, Resource resource)
    {
        resource.SetValue(number / resource.value);
        return resource;
    }
}