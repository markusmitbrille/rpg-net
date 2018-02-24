using UnityEngine;

public class Parent<T> : Dependency<T> where T : MonoBehaviour
{
    public Parent(MonoBehaviour child) : base(child)
    {
    }

    protected override T GetInstance(MonoBehaviour child) => child.GetComponentInParent<T>();
}