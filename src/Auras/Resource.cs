using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[Serializable]
[DataContract]
public class Resource : Stat
{
    [SerializeField]
    [DataMember]
    private float value;

    [SerializeField]
    [DataMember]
    private ComplexStat def = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat min = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat max = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat regen = new ComplexStat();

    public override float Value => value;

    public ComplexStat Def => def;
    public ComplexStat Min => min;
    public ComplexStat Max => max;
    public ComplexStat Regen => regen;

    public float Improvement => Arith.Avg(def.Improvement, min.Improvement, max.Improvement, regen.Improvement);
    public float ActualImprovement => Arith.Avg(def.ActualImprovement, min.ActualImprovement, max.ActualImprovement, regen.ActualImprovement);

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

    public void Reset() => SetValue(def);

    public void Regenerate() => SetValue(value + regen * Time.deltaTime);

    public void SetValue(float value)
    {
        if (min > max)
        {
            this.value = value;
        }
        else
        {
            this.value = Mathf.Clamp(value, min, max);
        }
    }
}