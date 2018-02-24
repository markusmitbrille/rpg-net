using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System.Collections.ObjectModel;
using UnityEngine;

[DataContract]
public sealed class Skill : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    public SkillInfo info;

    [DataMember]
    public float Cooldown { get; set; }

    public SkillInfo Info => info;

    public Actor Owner { get; private set; }

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

        return Owner.SendSpell(this, null, Owner, info.Active);
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

        return Owner.SendSpell(this, null, Owner, info.Passive);
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

    public class Collection : KeyedCollection<SkillInfo, Skill>
    {
        public Collection() : base(new IdentityEqualityComparer<SkillInfo>())
        {
        }

        protected override SkillInfo GetKeyForItem(Skill item) => item.Info;

        protected override void InsertItem(int index, Skill item)
        {
            if (item == null)
            {
                return;
            }
            if (Contains(item))
            {
                Destroy(item);
                return;
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Skill item) => InsertItem(index, item);

        protected override void RemoveItem(int index)
        {
            if (index >= Count)
            {
                return;
            }

            Destroy(this[index]);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (Skill item in this)
            {
                Destroy(item);
            }

            base.ClearItems();
        }
    }
}