using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System.Collections.ObjectModel;
using UnityEngine;

[DataContract]
public sealed class Resource : Stat
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

    private void Start() => Owner.Resources.Add(this);

    private void OnDestroy() => Owner.Resources.Remove(this);

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

    public class Collection : KeyedCollection<StatInfo, Resource>
    {
        public Collection() : base(new IdentityEqualityComparer<StatInfo>())
        {
        }

        protected override StatInfo GetKeyForItem(Resource item) => item.Info;

        protected override void InsertItem(int index, Resource item)
        {
            if (item == null)
            {
                return;
            }
            if (Contains(item))
            {
                Resource original = this[item.Info];
                original.Set(original.value + item.value);
                Destroy(item);
                return;
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Resource item) => InsertItem(index, item);

        protected override void RemoveItem(int index)
        {
            if (index >= Count)
            {
                return;
            }

            Destroy(this[index]);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (Resource item in this)
            {
                Destroy(item);
            }

            base.ClearItems();
        }
    }
}