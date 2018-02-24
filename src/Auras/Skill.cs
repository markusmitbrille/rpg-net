using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class Skill : MonoBehaviour, IUnique<SkillInfo>, IExtendable<Skill>, IDestructible
{
    [SerializeField]
    [DataMember]
    public SkillInfo id;

    [DataMember]
    public float Cooldown { get; set; }

    public SkillInfo ID => id;
    public Actor Owner { get; private set; }

    public bool IsOnCooldown => Cooldown > 0f;
    public bool IsActive => id.Active != null;
    public bool IsPassive => id.Passive != null;

    public bool Is(AuraCategory category) => (id.Active?.Is(category) ?? false) || (id.Passive?.Is(category) ?? false);

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

        return Owner.SendSpell(this, null, Owner, id.Active);
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

        return Owner.SendSpell(this, null, Owner, id.Passive);
    }

    public void Destruct() => Destroy(this);

    void IExtendable<Skill>.Extend(Skill other)
    {
    }

    private void Start()
    {
        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Bugger.Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destroy(this);
            return;
        }

        Owner.Skills.Add(this);
    }

    private void OnDestroy()
    {
        Owner.Skills.Remove(this);
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