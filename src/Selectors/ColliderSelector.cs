using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using UnityEngine;

[DataContract]
public abstract class ColliderSelector : Selector
{
    private Rigidbody body;

    [DataMember]
    private List<Actor> targets = new List<Actor>();

    public sealed override IEnumerable<Actor> Targets => targets.AsReadOnly();

    public Vector3 Aim => Owner.Aim;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Actor target in other.GetComponents<Actor>())
        {
            targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Actor target in other.GetComponents<Actor>())
        {
            targets.Remove(target);
        }
    }
}