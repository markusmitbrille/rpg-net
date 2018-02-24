using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class Equipment : MonoBehaviour, IUnique<EquipmentInfo>, IDestructible
{
    [SerializeField]
    [DataMember]
    private EquipmentInfo id;

    public EquipmentInfo ID => id;
    public Actor Owner { get; private set; }

    public bool Is(EquipmentCategory category) => id.Category.Is(category);

    public Equipment Equip(Actor actor)
    {
        if (!enabled)
        {
            return null;
        }

        Equipment equipment = Instantiate(this, actor.transform);

        if (id.Equip != null)
        {
            Owner.SendSpell(null, null, Owner, id.Equip);
        }

        return equipment;
    }

    public void Unequip()
    {
        if (!enabled)
        {
            return;
        }

        if (id.Unequip != null)
        {
            Owner.SendSpell(null, null, Owner, id.Unequip);
        }

        Destruct();
    }

    public void Destruct() => Destroy(this);

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
}