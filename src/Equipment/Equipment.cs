using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Equipment : MonoBehaviour, IIdentifiable<EquipmentInfo>
{
    [SerializeField]
    [DataMember]
    private EquipmentInfo info;

    [DataMember]
    private Actor actor;

    public EquipmentInfo Info => info;
    public Actor Actor => actor ?? (actor = GetComponent<Actor>());

    EquipmentInfo IIdentifiable<EquipmentInfo>.ID => Info;

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
            Actor.SendSpell(null, null, Actor, info.Equip);
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
            Actor.SendSpell(null, null, Actor, info.Unequip);
        }

        Destroy(this);
    }

    private void Start()
    {
        // Self-destruct if no owner was found to avoid orphaned equipments
        if (Actor == null)
        {
            Error($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }
    }
}