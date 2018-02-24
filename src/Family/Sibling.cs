using UnityEngine;

public class Sibling<T> : Dependency<T> where T : MonoBehaviour
{
    public Sibling(MonoBehaviour sibling) : base(sibling)
    {
    }

    protected override T GetInstance(MonoBehaviour sibling) => sibling.GetComponent<T>();
}