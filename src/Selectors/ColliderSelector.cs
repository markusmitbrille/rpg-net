using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DataContract]
public abstract class ColliderSelector : ConfirmedSelector
{
    [DataMember]
    private List<Actor> targets = new List<Actor>();

    public sealed override IEnumerable<Actor> Targets => targets.AsReadOnly();

    private void Start()
    {
        if (GetComponentsInChildren<Collider>().Any())
        {
            Rigidbody body = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
        }
    }

    private void Update()
    {
        transform.position = Position;
        transform.rotation = Rotation;
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