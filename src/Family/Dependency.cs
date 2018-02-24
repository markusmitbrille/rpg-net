using Autrage.LEX.NET;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public abstract class Dependency<T> where T : MonoBehaviour
{
    private MonoBehaviour dependent;
    private T instance;

    public T Instance => instance;

    public Dependency(MonoBehaviour dependent)
    {
        dependent.AssertNotNull(nameof(dependent));

        this.dependent = dependent;
    }

    public static implicit operator T(Dependency<T> dependency) => dependency.instance;

    public T Fetch()
    {
        instance = GetInstance(dependent);
        if (instance == null)
        {
            Bugger.Log($"{dependent.GetType()} {dependent} lost its {typeof(T)} {GetType()}.");
            UnityObject.Destroy(dependent);
        }

        return instance;
    }

    protected abstract T GetInstance(MonoBehaviour dependent);
}