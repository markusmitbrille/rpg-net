﻿using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Skill : MonoBehaviour
{
    private Actor owner;

    [SerializeField]
    [DataMember]
    private Aura active;

    [SerializeField]
    [DataMember]
    private Aura passive;

    [DataMember]
    public float Cooldown { get; set; }

    public Actor Owner => owner ?? (owner = GetComponent<Actor>());

    public Aura Active => active;
    public Aura Passive => passive;

    public bool HasActive => active != null;
    public bool HasPassive => passive != null;

    public Aura Use()
    {
        if (!HasActive)
        {
            return null;
        }
        if (Cooldown > 0f)
        {
            return null;
        }

        return Owner.SendSpell(this, null, Owner, active);
    }

    private void Start()
    {
        // Self-destruct if no owner was found to avoid orphaned skills
        if (Owner == null)
        {
            Error($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        if (Cooldown > 0f)
        {
            ReduceCooldown();
        }
    }

    private void ReduceCooldown()
    {
        Cooldown -= Time.deltaTime;
        if (Cooldown <= 0f)
        {
            Cooldown = 0f;
            UsePassive();
        }
    }

    private void UsePassive()
    {
        if (HasPassive)
        {
            Owner.SendSpell(this, null, Owner, passive);
        }
    }
}