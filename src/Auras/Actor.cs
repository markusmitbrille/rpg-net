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

    private Aura[] auras;

    public float Might => might;
    public float Armour => armour;
    public float Knowledge => knowledge;

    public float MinOffence => minOffence;
    public float MaxOffence => maxOffence;
    public float MinDefence => minDefence;
    public float MaxDefence => maxDefence;

    public float Initiative => initiative;
    public float Haste => haste;
    public float Speed => speed;

    public float Life => life;
    public float Aether => aether;
    public float Focus => focus;
    public float Vim => vim;

    public float OutOfCombatRegenIterations => outOfCombatRegenIterations;

    public float PrimaryImprovement => Arith.Avg(armour.Improvement, might.Improvement, knowledge.Improvement);
    public float SecondaryImprovement => Arith.Avg(initiative.Improvement, haste.Improvement, speed.Improvement);
    public float ResourceImprovement => Arith.Avg(life.Improvement, aether.Improvement, focus.Improvement, speed.Improvement);
    public float OffensiveImprovement => Arith.Avg(might.Improvement, knowledge.Improvement);
    public float DefensiveImprovement => Arith.Avg(armour.Improvement, knowledge.Improvement);

    public float Versatility => Armour == 0f && Might == 0f ? 0.5f : Might / (Might + Armour);
    public float Offence => Mathf.Max(0f, minOffence > maxOffence ? Versatility : minOffence + (maxOffence - minOffence) * Versatility);
    public float Defence => Mathf.Max(0f, minDefence > maxDefence ? Versatility : minDefence + (maxDefence - minDefence) * Versatility);

    [DataMember]
    public bool IsInCombat { get; set; }

    public Actor Target { get; private set; }

    public Aura[] Auras => auras ?? (auras = GetComponentsInChildren<Aura>());

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

        // Invoke event during which parameters may be adjusted
        DealingDamage?.Invoke(this, e);

        // Apply offence last
        e.Amount *= Offence;

        e.Target?.TakeDamage(e);

        // Invoke event during which parameters may be adjusted
        DealtDamage?.Invoke(this, e);
    }

    public void ApplyAura(Aura source, Actor target, Aura prefab)
    {
        AuraEventArgs e = new AuraEventArgs(source, this, target, prefab);

        // Invoke event during which parameters may be adjusted
        ApplyingAura?.Invoke(this, e);

        e.Target?.SufferAura(e);

        // Invoke event during which parameters may be adjusted
        AppliedAura?.Invoke(this, e);
    }

    private void Update()
    {
        // Refresh auras once per tick
        auras = null;

        RegenerateResources();
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

        // Invoke event during which parameters may be adjusted
        TakingDamage?.Invoke(this, e);

        // Apply defence last
        e.Amount *= Defence;

        float oldLifeValue = Life;
        life += e.Amount;
        e.Amount = Life - oldLifeValue;

        // Invoke event during which parameters may be adjusted
        TookDamage?.Invoke(this, e);
    }

    private void SufferAura(AuraEventArgs e)
    {
        e.AssertNotNull(nameof(e));

        // Invoke event during which parameters may be adjusted
        SufferingAura?.Invoke(this, e);

        if (e.Prefab != null)
        {
            Instantiate(e.Prefab, transform);
        }

        // Invoke event during which parameters may be adjusted
        SufferedAura?.Invoke(this, e);
    }
}