using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[DataContract]
public class Actor : MonoBehaviour
{
    private const int damageTimeToLive = 255;
    private const int auraTimeToLive = 255;

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

    public ComplexStat Might => might;
    public ComplexStat Armour => armour;
    public ComplexStat Knowledge => knowledge;

    public ComplexStat MinOffence => minOffence;
    public ComplexStat MaxOffence => maxOffence;
    public ComplexStat MinDefence => minDefence;
    public ComplexStat MaxDefence => maxDefence;

    public ComplexStat Initiative => initiative;
    public ComplexStat Haste => haste;
    public ComplexStat Speed => speed;

    public Resource Life => life;
    public Resource Aether => aether;
    public Resource Focus => focus;
    public Resource Vim => vim;

    public ComplexStat OutOfCombatRegenIterations => outOfCombatRegenIterations;

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

    public event EventHandler<PreDamageEventArgs> SendingDamage;
    public event EventHandler<PostDamageEventArgs> SentDamage;

    public event EventHandler<PreDamageEventArgs> ReceivingDamage;
    public event EventHandler<PostDamageEventArgs> ReceivedDamage;

    public event EventHandler<PreAuraEventArgs> SendingAura;
    public event EventHandler<PostAuraEventArgs> SentAura;

    public event EventHandler<PreAuraEventArgs> ReceivingAura;
    public event EventHandler<PostAuraEventArgs> ReceivedAura;

    public void SendDamage(Aura source, Actor receiver, DamageType type, float amount) => SendDamage(source, receiver, type, amount, damageTimeToLive);

    public void SendAura(Aura source, Actor receiver, Aura prefab) => SendAura(source, receiver, prefab, auraTimeToLive);

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

    private void SendDamage(Aura source, Actor receiver, DamageType type, float amount, int ttl)
    {
        if (ttl < 0)
        {
            return;
        }

        PreDamageEventArgs e = new PreDamageEventArgs(source, this, receiver, type, amount);

        SendingDamage?.Invoke(this, e);
        if (e.IsInvalid)
        {
            return;
        }

        e.Receiver.ReceiveDamage(source, this, e.Type, e.Amount, ttl);
    }

    private void ReceiveDamage(Aura source, Actor sender, DamageType type, float amount, int ttl)
    {
        PreDamageEventArgs e = new PreDamageEventArgs(source, sender, this, type, amount);

        ReceivingDamage?.Invoke(this, e);
        if (e.IsInvalid)
        {
            return;
        }
        if (e.Receiver != this)
        {
            SendDamage(source, e.Receiver, e.Type, e.Amount, --ttl);
            return;
        }

        float old = life;
        life.Set(life + e.Amount);
        e.Amount = life - old;

        ReceivedDamage?.Invoke(this, new PostDamageEventArgs(source, sender, this, e.Type, e.Amount));
        sender.SentDamage?.Invoke(sender, new PostDamageEventArgs(source, sender, this, e.Type, e.Amount));
    }

    private void SendAura(Aura source, Actor receiver, Aura prefab, int ttl)
    {
        if (ttl < 0)
        {
            return;
        }

        PreAuraEventArgs e = new PreAuraEventArgs(source, this, receiver, prefab);

        SendingAura?.Invoke(this, e);
        if (e.IsInvalid)
        {
            return;
        }

        e.Receiver.ReceiveAura(source, this, e.Prefab, ttl);
    }

    private void ReceiveAura(Aura source, Actor sender, Aura prefab, int ttl)
    {
        PreAuraEventArgs e = new PreAuraEventArgs(source, sender, this, prefab);
        ReceivingAura?.Invoke(this, e);
        if (e.IsInvalid)
        {
            return;
        }
        if (e.Receiver != this)
        {
            SendAura(source, e.Receiver, e.Prefab, --ttl);
            return;
        }

        Instantiate(e.Prefab, transform);

        ReceivedAura?.Invoke(this, new PostAuraEventArgs(source, sender, this, e.Prefab));
        sender.SentAura?.Invoke(sender, new PostAuraEventArgs(source, sender, this, e.Prefab));
    }
}