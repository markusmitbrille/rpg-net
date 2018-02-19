using Autrage.LEX.NET.Serialization;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private Actor owner;

    [SerializeField]
    [DataMember]
    private Aura active;

    [SerializeField]
    [DataMember]
    private Aura passive;

    [DataMember]
    public float Cooldown { get; set; }

    public Actor Owner => owner ?? (owner = GetComponent<Actor>());

    public Aura Active => active;
    public Aura Passive => passive;

    public bool HasActive => active != null;
    public bool HasPassive => passive != null;

    public Aura Use()
    {
        if (!HasActive)
        {
            return null;
        }
        if (Cooldown > 0f)
        {
            return null;
        }

        return Owner.SendSpell(this, null, Owner, active);
    }

    private void Update()
    {
        // Refresh owner once per tick
        owner = null;

        // Destroy if no owner was found to avoid orphaned skills
        if (Owner == null)
        {
            Destroy(this);
            return;
        }

        if (Cooldown > 0f)
        {
            Cooldown -= Time.deltaTime;
        }
        if (Cooldown > 0f)
        {
            return;
        }

        Cooldown = 0f;

        if (HasPassive)
        {
            Owner.SendSpell(this, null, Owner, passive);
        }
    }
}
