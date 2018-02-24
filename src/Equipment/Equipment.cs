using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public sealed class Equipment : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private EquipmentInfo info;

    public EquipmentInfo Info => info;
    public Parent<Actor> Owner { get; private set; }

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
            Owner.Instance.SendSpell(null, null, Owner, info.Equip);
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
            Owner.Instance.SendSpell(null, null, Owner, info.Unequip);
        }

        Destroy(this);
    }

    private void Awake()
    {
        Owner = new Parent<Actor>(this);
    }

    private void Start()
    {
        Owner.Fetch();
        Owner.Instance?.Equipments.Fetch();
    }

    private void OnDestroy()
    {
        Owner.Instance?.Equipments.Fetch();
    }
}