using UnityEngine;

public class Child<T> : Dependency<T> where T : MonoBehaviour
{
    public Child(MonoBehaviour parent) : base(parent)
    {
    }

    protected override T GetInstance(MonoBehaviour parent) => parent.GetComponentInChildren<T>();
}