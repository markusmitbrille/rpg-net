using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Family<T> : IEnumerable<T>, IEnumerable where T : MonoBehaviour
{
    private MonoBehaviour parent;
    private T[] children;

    public IEnumerable<T> Children => children.ToList().AsReadOnly();

    public Family(MonoBehaviour parent)
    {
        this.parent = parent;
    }

    public void Fetch() => children = parent.GetComponentsInChildren<T>();

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T child in children)
        {
            yield return child;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}