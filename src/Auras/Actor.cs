using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

[DisallowMultipleComponent]
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

    public event EventHandler<PackageEventArgs> SendingPackage;
    public event EventHandler<ReportEventArgs> SentPackage;

    public event EventHandler<PackageEventArgs> ReceivingPackage;
    public event EventHandler<ReportEventArgs> ReceivedPackage;

    public override string ToString() => name;

    public void SendDamage(Skill origin, Aura source, Actor receiver, DamageType type, float amount) => SendPackage(new Damage(origin, source, this) { Receiver = receiver, Type = type, Amount = amount });

    public void SendSpell(Skill origin, Aura source, Actor receiver, Aura prefab) => SendPackage(new Spell(origin, source, this) { Receiver = receiver, Prefab = prefab });

    public void SendPackage(Package package)
    {
        if (!package.IsValid)
        {
            return;
        }

        SendingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return;
        }
        if (package.Sender != this)
        {
            package.Sender.SendPackage(package);
            return;
        }

        package.Receiver.ReceivePackage(package);
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

    private void ReceivePackage(Package package)
    {
        if (!package.IsValid)
        {
            return;
        }

        ReceivingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return;
        }
        if (package.Receiver != this)
        {
            package.Tick();
            package.Sender.SendPackage(package);
            return;
        }

        Report report = package.Unwrap();

        ReceivedPackage?.Invoke(this, new ReportEventArgs(report));
        report.Sender.SentPackage?.Invoke(report.Sender, new ReportEventArgs(report));
    }
}