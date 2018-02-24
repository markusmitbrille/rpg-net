﻿using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[DataContract]
public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private ActorInfo id;

    public ActorInfo ID => id;

    public Skill.Collection Skills { get; private set; }
    public Equipment.Collection Equipments { get; private set; }
    public ComplexStat.Collection Complices { get; private set; }
    public Resource.Collection Resources { get; private set; }

    public abstract Actor Target { get; }
    public abstract Vector3 Aim { get; }
    public abstract bool IsInCombat { get; }

    public event EventHandler<PackageEventArgs> SendingPackage;

    public event EventHandler<ReportEventArgs> SentPackage;

    public event EventHandler<PackageEventArgs> ReceivingPackage;

    public event EventHandler<ReportEventArgs> ReceivedPackage;

    public override string ToString() => name;

    public void ConfirmSelection() => ExecuteEvents.Execute<IConfirmSelectionTarget>(gameObject, null, (target, data) => target.ConfirmSelection());

    public bool SendAttack(Skill origin, Aura source, Actor receiver, Damage damage) => SendPackage(new Attack(origin, source, this) { Receiver = receiver, Damage = damage });

    public Aura SendSpell(Skill origin, Aura source, Actor receiver, Aura prefab) => SendPackage(new Spell(origin, source, this) { Receiver = receiver, Prefab = prefab });

    public T SendPackage<T>(Package<T> package)
    {
        if (!package.IsValid)
        {
            return default(T);
        }

        SendingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return default(T);
        }
        if (package.Sender != this)
        {
            package.Sender.SendPackage(package);
            return default(T);
        }

        return package.Receiver.ReceivePackage(package);
    }

    public object SendPackage(IPackage package)
    {
        if (!package.IsValid)
        {
            return null;
        }

        SendingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return null;
        }
        if (package.Sender != this)
        {
            package.Sender.SendPackage(package);
            return null;
        }

        return package.Receiver.ReceivePackage(package);
    }

    private void Awake()
    {
        Skills = new Skill.Collection();
        Equipments = new Equipment.Collection();
        Complices = new ComplexStat.Collection();
        Resources = new Resource.Collection();
    }

    private T ReceivePackage<T>(Package<T> package)
    {
        if (!package.IsValid)
        {
            return default(T);
        }

        ReceivingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return default(T);
        }
        if (package.Receiver != this)
        {
            package.Tick();
            package.Sender.SendPackage(package);
            return default(T);
        }

        Report<T> report = package.Unwrap();

        ReceivedPackage?.Invoke(this, new ReportEventArgs(report));
        report.Sender.SentPackage?.Invoke(report.Sender, new ReportEventArgs(report));

        return report.Content;
    }

    private object ReceivePackage(IPackage package)
    {
        if (!package.IsValid)
        {
            return null;
        }

        ReceivingPackage?.Invoke(this, new PackageEventArgs(package));
        if (!package.IsValid)
        {
            return null;
        }
        if (package.Receiver != this)
        {
            package.Tick();
            package.Sender.SendPackage(package);
            return null;
        }

        IReport report = package.Unwrap();

        ReceivedPackage?.Invoke(this, new ReportEventArgs(report));
        report.Sender.SentPackage?.Invoke(report.Sender, new ReportEventArgs(report));

        return report.Content;
    }
}