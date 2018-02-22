using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Skill : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private SkillInfo info;

    private Actor owner;

    public SkillInfo Info => info;

    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());

    [DataMember]
    public float Cooldown { get; set; }

    public bool Is(AuraCategory category) => (info.Active?.Is(category) ?? false) || (info.Passive?.Is(category) ?? false);

    public Aura Use()
    {
        if (!enabled)
        {
            return null;
        }
        if (info.Active == null)
        {
            return null;
        }
        if (Cooldown > 0f)
        {
            return null;
        }

        return Owner.SendSpell(this, null, Owner, info.Active);
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

        UsePassive();
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
        if (!enabled)
        {
            return;
        }
        if (info.Passive == null)
        {
            return;
        }
        if (Cooldown > 0f)
        {
            return;
        }

        Owner.SendSpell(this, null, Owner, info.Passive);
    }
}