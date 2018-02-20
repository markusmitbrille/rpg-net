using Autrage.LEX.NET.Serialization;
using System.Collections.Generic;
using UnityEngine;

[DataContract]
public abstract class ColliderSelector<T> : Selector<T> where T : MonoBehaviour
{
    private Rigidbody Rigidbody;

    [DataMember]
    private List<T> targets = new List<T>();

    public override IEnumerator<T> GetEnumerator() => targets.GetEnumerator();

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody == null)
        {
            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (T target in other.GetComponents<T>())
        {
            targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (T target in other.GetComponents<T>())
        {
            targets.Remove(target);
        }
    }
}