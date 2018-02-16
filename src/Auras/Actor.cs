using UnityEngine;
using System;

public class Actor : DataDrivenBehaviour
{
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

        public bool IsDamage { get { return Amount > 0f; } }
        public bool IsHealing { get { return Amount < 0f; } }

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

    [Header("Primary Attributes")]
    [SerializeField]
    [DataMember]
    private ComplexStat might = new ComplexStat();
    public float Might { get { return Mathf.Max(0f, might?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat armour = new ComplexStat();
    public float Armour { get { return Mathf.Max(0f, armour?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat knowledge = new ComplexStat();
    public float Knowledge { get { return Mathf.Max(0f, knowledge?.Value ?? 0f); } }

    public float PrimaryImprovement { get { return (armour.Improvement + might.Improvement + knowledge.Improvement) / 3f; } }
    public float OffensiveImprovement { get { return (might.Improvement + knowledge.Improvement) / 2f; } }
    public float DefensiveImprovement { get { return (armour.Improvement + knowledge.Improvement) / 2f; } }

    [Header("Versatility")]
    [SerializeField]
    [DataMember]
    private ComplexStat minOffence = new ComplexStat();
    public float MinOffence { get { return Mathf.Max(0f, minOffence?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat maxOffence = new ComplexStat();
    public float MaxOffence { get { return Mathf.Max(0f, maxOffence?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat minDefence = new ComplexStat();
    public float MinDefence { get { return Mathf.Max(0f, minDefence?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat maxDefence = new ComplexStat();
    public float MaxDefence { get { return Mathf.Max(0f, maxDefence?.Value ?? 0f); } }

    public float Versatility { get { return Armour == 0f && Might == 0f ? 0.5f : Might / (Might + Armour); } }
    public float Offence { get { return Mathf.Max(0f, MinOffence > MaxOffence ? Versatility : MinOffence + (MaxOffence - MinOffence) * Versatility); } }
    public float Defence { get { return Mathf.Max(0f, MinDefence > MaxDefence ? Versatility : MinDefence + (MaxDefence - MinDefence) * Versatility); } }

    [Header("Secondary Attributes")]
    [SerializeField]
    [DataMember]
    private ComplexStat initiative = new ComplexStat();
    public float Initiative { get { return initiative?.Value ?? 0f; } }

    [SerializeField]
    [DataMember]
    private ComplexStat haste = new ComplexStat();
    public float Haste { get { return Mathf.Max(0f, haste?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private ComplexStat speed = new ComplexStat();
    public float Speed { get { return Mathf.Max(0f, speed?.Value ?? 0f); } }

    public float SecondaryImprovement { get { return (initiative.Improvement + haste.Improvement + speed.Improvement) / 3f; } }

    [Header("Resources")]
    [SerializeField]
    [DataMember]
    private Resource life = new Resource();
    public float Life { get { return Mathf.Max(0f, life?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private Resource aether = new Resource();
    public float Aether { get { return Mathf.Max(0f, aether?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private Resource focus = new Resource();
    public float Focus { get { return Mathf.Max(0f, focus?.Value ?? 0f); } }

    [SerializeField]
    [DataMember]
    private Resource vim = new Resource();
    public float Vim { get { return Mathf.Max(0f, vim?.Value ?? 0f); } }

    [Header("Minor Stats")]
    [SerializeField]
    [DataMember]
    private ComplexStat outOfCombatRegenIterations = new ComplexStat();
    public float OutOfCombatRegenIterations { get { return Mathf.Max(0f, outOfCombatRegenIterations?.Value ?? 0f); } }

    [DataMember]
    public bool IsInCombat { get; set; }

    public Actor Target { get; private set; }

    private Aura[] auras = null;
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

    private void Update()
    {
        // Refresh auras once per tick
        auras = null;

        RegenerateResources();

        // Reset focus

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

    public void ApplyAura(Aura source, Actor target, Aura prefab)
    {
        AuraEventArgs e = new AuraEventArgs(source, this, target, prefab);

        // Invoke event during which parameters my be adjusted
        ApplyingAura?.Invoke(this, e);

        e.Target?.SufferAura(e);

        // Invoke event during which parameters my be adjusted
        AppliedAura?.Invoke(this, e);
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
}
