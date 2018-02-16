using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[DataContract]
public class Actor : MonoBehaviour
{
    [Header("Primary Attributes")]
    [SerializeField]
    [DataMember]
    private ComplexStat might = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat armour = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat knowledge = new ComplexStat();

    [Header("Versatility")]
    [SerializeField]
    [DataMember]
    private ComplexStat minOffence = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat maxOffence = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat minDefence = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat maxDefence = new ComplexStat();

    [Header("Secondary Attributes")]
    [SerializeField]
    [DataMember]
    private ComplexStat initiative = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat haste = new ComplexStat();

    [SerializeField]
    [DataMember]
    private ComplexStat speed = new ComplexStat();

    [Header("Resources")]
    [SerializeField]
    [DataMember]
    private Resource life = new Resource();

    [SerializeField]
    [DataMember]
    private Resource aether = new Resource();

    [SerializeField]
    [DataMember]
    private Resource focus = new Resource();

    [SerializeField]
    [DataMember]
    private Resource vim = new Resource();

    [Header("Minor Stats")]
    [SerializeField]
    [DataMember]
    private ComplexStat outOfCombatRegenIterations = new ComplexStat();

    private Aura[] auras = null;

    public float Might => Mathf.Max(0f, might?.Value ?? 0f);

    public float Armour => Mathf.Max(0f, armour?.Value ?? 0f);

    public float Knowledge => Mathf.Max(0f, knowledge?.Value ?? 0f);

    public float PrimaryImprovement => (armour.Improvement + might.Improvement + knowledge.Improvement) / 3f;

    public float OffensiveImprovement => (might.Improvement + knowledge.Improvement) / 2f;

    public float DefensiveImprovement => (armour.Improvement + knowledge.Improvement) / 2f;

    public float MinOffence => Mathf.Max(0f, minOffence?.Value ?? 0f);

    public float MaxOffence => Mathf.Max(0f, maxOffence?.Value ?? 0f);

    public float MinDefence => Mathf.Max(0f, minDefence?.Value ?? 0f);

    public float MaxDefence => Mathf.Max(0f, maxDefence?.Value ?? 0f);

    public float Versatility => Armour == 0f && Might == 0f ? 0.5f : Might / (Might + Armour);

    public float Offence => Mathf.Max(0f, MinOffence > MaxOffence ? Versatility : MinOffence + (MaxOffence - MinOffence) * Versatility);

    public float Defence => Mathf.Max(0f, MinDefence > MaxDefence ? Versatility : MinDefence + (MaxDefence - MinDefence) * Versatility);

    public float Initiative => initiative?.Value ?? 0f;

    public float Haste => Mathf.Max(0f, haste?.Value ?? 0f);

    public float Speed => Mathf.Max(0f, speed?.Value ?? 0f);

    public float SecondaryImprovement => (initiative.Improvement + haste.Improvement + speed.Improvement) / 3f;

    public float Life => Mathf.Max(0f, life?.Value ?? 0f);

    public float Aether => Mathf.Max(0f, aether?.Value ?? 0f);

    public float Focus => Mathf.Max(0f, focus?.Value ?? 0f);

    public float Vim => Mathf.Max(0f, vim?.Value ?? 0f);

    public float OutOfCombatRegenIterations => Mathf.Max(0f, outOfCombatRegenIterations?.Value ?? 0f);

    [DataMember]
    public bool IsInCombat { get; set; }

    public Actor Target { get; private set; }

    public Aura[] Auras
    {
        get
        {
            if (auras == null)
            {
                auras = GetComponentsInChildren<Aura>();
            }

            return auras;
        }
    }

    public event EventHandler<DamageEventArgs> DealingDamage;

    public event EventHandler<DamageEventArgs> DealtDamage;

    public event EventHandler<DamageEventArgs> TakingDamage;

    public event EventHandler<DamageEventArgs> TookDamage;

    public event EventHandler<AuraEventArgs> ApplyingAura;

    public event EventHandler<AuraEventArgs> AppliedAura;

    public event EventHandler<AuraEventArgs> SufferingAura;

    public event EventHandler<AuraEventArgs> SufferedAura;

    public void DealDamage(Aura source, Actor target, DamageType type, float amount)
    {
        DamageEventArgs e = new DamageEventArgs(source, this, target, type, amount);

        // Invoke event during which parameters my be adjusted
        DealingDamage?.Invoke(this, e);

        // Apply offence last
        e.Amount *= Offence;

        e.Target?.TakeDamage(e);

        // Invoke event during which parameters my be adjusted
        DealtDamage?.Invoke(this, e);
    }

    public void ApplyAura(Aura source, Actor target, Aura prefab)
    {
        AuraEventArgs e = new AuraEventArgs(source, this, target, prefab);

        // Invoke event during which parameters my be adjusted
        ApplyingAura?.Invoke(this, e);

        e.Target?.SufferAura(e);

        // Invoke event during which parameters my be adjusted
        AppliedAura?.Invoke(this, e);
    }

    private void Update()
    {
        // Refresh auras once per tick
        auras = null;

        RegenerateResources();

        throw new NotImplementedException("Regen focus");
    }

    private void RegenerateResources()
    {
        int iteration = 0;
        int iterations = Mathf.RoundToInt(OutOfCombatRegenIterations);
        do
        {
            life.Regenerate();
            aether.Regenerate();
            focus.Regenerate();
            vim.Regenerate();

            iteration++;
        } while (!IsInCombat && iteration < iterations);
    }

    private void TakeDamage(DamageEventArgs e)
    {
        e.AssertNotNull(nameof(e));

        // Invoke event during which parameters my be adjusted
        TakingDamage?.Invoke(this, e);

        // Apply defence last
        e.Amount *= Defence;

        float oldLifeValue = Life;
        life += e.Amount;
        e.Amount = Life - oldLifeValue;

        // Invoke event during which parameters my be adjusted
        TookDamage?.Invoke(this, e);
    }

    private void SufferAura(AuraEventArgs e)
    {
        e.AssertNotNull(nameof(e));

        // Invoke event during which parameters my be adjusted
        SufferingAura?.Invoke(this, e);

        if (e.Prefab != null)
        {
            Instantiate(e.Prefab, transform);
        }

        // Invoke event during which parameters my be adjusted
        SufferedAura?.Invoke(this, e);
    }

    public class DamageEventArgs : EventArgs
    {
        public Aura Source { get; }
        public Actor Origin { get; }

        public Actor OriginalTarget { get; }
        public DamageType OriginalType { get; }
        public float OriginalAmount { get; }

        public Actor Target { get; set; }
        public DamageType Type { get; set; }
        public float Amount { get; set; }

        public bool IsDamage => Amount > 0f;
        public bool IsHealing => Amount < 0f;

        public DamageEventArgs(Aura source, Actor origin, Actor target, DamageType type, float amount)
        {
            origin.AssertNotNull(nameof(origin));
            target.AssertNotNull(nameof(target));

            Source = source;
            Origin = origin;

            OriginalTarget = target;
            OriginalType = type;
            OriginalAmount = amount;

            Target = target;
            Type = type;
            Amount = amount;
        }
    }

    public class AuraEventArgs : EventArgs
    {
        public Aura Source { get; }
        public Actor Origin { get; }

        public Actor OriginalTarget { get; }
        public Aura OriginalPrefab { get; }

        public Actor Target { get; set; }
        public Aura Prefab { get; set; }

        public AuraEventArgs(Aura source, Actor origin, Actor target, Aura prefab)
        {
            origin.AssertNotNull(nameof(origin));
            target.AssertNotNull(nameof(target));
            prefab.AssertNotNull(nameof(prefab));

            Source = source;
            Origin = origin;

            OriginalTarget = target;
            OriginalPrefab = prefab;

            Target = target;
            Prefab = prefab;
        }
    }
}