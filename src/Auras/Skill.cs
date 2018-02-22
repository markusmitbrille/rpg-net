using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Skill : MonoBehaviour, IIdentifiable<SkillInfo>
{
    [SerializeField]
    [DataMember]
    public SkillInfo info;

    [DataMember]
    private Actor actor;

    public SkillInfo Info => info;
    public Actor Actor => actor ?? (actor = GetComponentInParent<Actor>());

    [DataMember]
    public float Cooldown { get; set; }

    SkillInfo IIdentifiable<SkillInfo>.ID => Info;

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

        return Actor.SendSpell(this, null, Actor, info.Active);
    }

    private void Start()
    {
        // Self-destruct if no owner was found to avoid orphaned skills
        if (Actor == null)
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

        Actor.SendSpell(this, null, Actor, info.Passive);
    }
}