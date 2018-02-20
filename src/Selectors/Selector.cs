using Autrage.LEX.NET.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[DataContract]
public abstract class Selector<T> : MonoBehaviour, IEnumerable<T>, IEnumerable where T : MonoBehaviour
{
    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (T target in this)
        {
            builder.Append(target.ToString());
        }

        return builder.ToString();
    }
}
