using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Equipment : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private EquipmentInfo info;

    private Actor owner;

    public EquipmentInfo Info => info;

    public Actor Owner => owner ?? (owner = GetComponent<Actor>());

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
        // Self-destruct if no owner was found to avoid orphaned equipments
        if (Owner == null)
        {
            Error($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }
    }
}