using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class Resource : Stat, IExtendable<Resource>
{
    [SerializeField]
    [DataMember]
    private float value;

    [SerializeField]
    [DataMember]
    private ComplexStat def;

    [SerializeField]
    [DataMember]
    private ComplexStat min;

    [SerializeField]
    [DataMember]
    private ComplexStat max;

    [SerializeField]
    [DataMember]
    private ComplexStat regen;

    [SerializeField]
    [DataMember]
    private ComplexStat combatRegen;

    public override float Value => value;

    public ComplexStat Def => def;
    public ComplexStat Min => min;
    public ComplexStat Max => max;
    public ComplexStat Regen => regen;
    public ComplexStat CombatRegen => combatRegen;

    public float Improvement => Arith.Avg(def?.Improvement ?? 0, min?.Improvement ?? 0, max?.Improvement ?? 0, regen?.Improvement ?? 0, combatRegen?.Improvement ?? 0);
    public float ActualImprovement => Arith.Avg(def?.ActualImprovement ?? 0, min?.ActualImprovement ?? 0, max?.ActualImprovement ?? 0, regen?.ActualImprovement ?? 0, combatRegen?.Improvement ?? 0);

    public void Set(float value)
    {
        if (min > max)
        {
            this.value = value;
        }
        else
        {
            this.value = Mathf.Clamp(value, min ?? 0f, max ?? 0f);
        }
    }

    public void Clear() => Set(def ?? 0f);

    public void Extend(Resource other) => Set(value + other.value);

    protected override void Incorporate() => Owner.Resources.Add(this);

    protected override void Excorporate() => Owner.Resources.Remove(this);

    private void Awake() => value = def ?? 0f;

    private void Update()
    {
        if (Owner.IsInCombat)
        {
            RegenerateCombat();
        }
        else
        {
            Regenerate();
        }
    }

    private void Regenerate() => Set(value + (regen ?? 0f) * Time.deltaTime);

    private void RegenerateCombat() => Set(value + (combatRegen ?? 0f) * Time.deltaTime);
}