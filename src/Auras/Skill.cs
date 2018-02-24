using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class Skill : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private SkillInfo info;

    [DataMember]
    public float Cooldown { get; set; }

    public SkillInfo Info => info;
    public Parent<Actor> Owner { get; private set; }

    public bool IsOnCooldown => Cooldown > 0f;
    public bool IsActive => info.Active != null;
    public bool IsPassive => info.Passive != null;

    public bool Is(AuraCategory category) => (info.Active?.Is(category) ?? false) || (info.Passive?.Is(category) ?? false);

    public Aura SendActive()
    {
        if (!enabled)
        {
            return null;
        }
        if (!IsActive)
        {
            return null;
        }
        if (IsOnCooldown)
        {
            return null;
        }

        return Owner.Instance.SendSpell(this, null, Owner, info.Active);
    }

    public Aura SendPassive()
    {
        if (!enabled)
        {
            return null;
        }
        if (!IsPassive)
        {
            return null;
        }
        if (IsOnCooldown)
        {
            return null;
        }

        return Owner.Instance.SendSpell(this, null, Owner, info.Passive);
    }

    private void Awake()
    {
        Owner = new Parent<Actor>(this);
    }

    private void Start()
    {
        Owner.Fetch();
        Owner.Instance?.Skills.Fetch();
    }

    private void OnDestroy()
    {
        Owner.Instance?.Skills.Fetch();
    }

    private void Update()
    {
        if (IsOnCooldown)
        {
            ReduceCooldown();
        }

        SendPassive();
    }

    private void ReduceCooldown() => Cooldown = Cooldown > Time.deltaTime ? Cooldown - Time.deltaTime : 0f;
}