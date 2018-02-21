using Autrage.LEX.NET.Serialization;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public class Equipment : MonoBehaviour
{
    private Actor owner;

    [SerializeField]
    [DataMember]
    private Aura equip;

    [SerializeField]
    [DataMember]
    private Aura unequip;

    public Actor Owner => owner ?? (owner = GetComponent<Actor>());

    public Equipment Equip(Actor actor)
    {
        if (!enabled)
        {
            return null;
        }

        Equipment equipment = Instantiate(this, actor.transform);

        if (equip != null)
        {
            Owner.SendSpell(null, null, Owner, equip);
        }

        return equipment;
    }

    public void Unequip()
    {
        if (!enabled)
        {
            return;
        }

        if (unequip != null)
        {
            Owner.SendSpell(null, null, Owner, unequip);
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