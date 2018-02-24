using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System.Collections.ObjectModel;
using UnityEngine;

[DataContract]
public sealed class Equipment : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private EquipmentInfo info;

    public EquipmentInfo Info => info;
    public Actor Owner { get; private set; }

    public bool Is(EquipmentCategory category) => info.Category.Is(category);

    public Equipment Equip(Actor actor)
    {
        if (!enabled)
        {
            return null;
        }

        Equipment equipment = Instantiate(this, actor.transform);

        if (info.Equip != null)
        {
            Owner.SendSpell(null, null, Owner, info.Equip);
        }

        return equipment;
    }

    public void Unequip()
    {
        if (!enabled)
        {
            return;
        }

        if (info.Unequip != null)
        {
            Owner.SendSpell(null, null, Owner, info.Unequip);
        }

        Destroy(this);
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

        Owner.Equipments.Add(this);
    }

    private void OnDestroy()
    {
        Owner.Equipments.Remove(this);
    }

    public class Collection : KeyedCollection<EquipmentInfo, Equipment>
    {
        public Collection() : base(new IdentityEqualityComparer<EquipmentInfo>())
        {
        }

        protected override EquipmentInfo GetKeyForItem(Equipment item) => item.Info;

        protected override void InsertItem(int index, Equipment item)
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

        protected override void SetItem(int index, Equipment item) => InsertItem(index, item);

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
            foreach (Equipment item in this)
            {
                Destroy(item);
            }

            base.ClearItems();
        }
    }
}