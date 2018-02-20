using Autrage.LEX.NET.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[DataContract]
public abstract class Selector<T> : MonoBehaviour, IEnumerable<T>, IEnumerable where T : MonoBehaviour
{
    public List<T> Targets => this.ToList();

    public override string ToString()
    {
        const string seperator = ", ";

        StringBuilder builder = new StringBuilder();
        foreach (T target in this)
        {
            builder.Append(target.ToString()).Append(seperator);
        }

        if (builder.Length >= seperator.Length)
        {
            builder.Remove(builder.Length - seperator.Length, seperator.Length);
        }
        return builder.ToString();
    }

    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}