using Autrage.LEX.NET.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DataContract]
public abstract class Selector : MonoBehaviour, IEnumerable<Actor>, IEnumerable
{
    private Actor owner;
    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());

    public abstract IEnumerable<Actor> Targets { get; }
    public abstract bool HasTargets { get; }

    public override string ToString()
    {
        const string seperator = ", ";

        StringBuilder builder = new StringBuilder();
        foreach (Actor target in this)
        {
            builder.Append(target.ToString()).Append(seperator);
        }

        if (builder.Length < seperator.Length)
        {
            return "Nobody";
        }
        else
        {
            builder.Remove(builder.Length - seperator.Length, seperator.Length);
            return builder.ToString();
        }
    }

    IEnumerator<Actor> IEnumerable<Actor>.GetEnumerator() => Targets.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Targets.GetEnumerator();

    private void Update()
    {
        // Refresh owner once per tick
        owner = null;

        // Destroy if no owner was found to avoid orphaned selectors
        if (Owner == null)
        {
            Warning($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }
    }
}